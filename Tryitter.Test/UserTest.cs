using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Tryitter.Repositories;
using Tryitter.Models;
using Microsoft.Extensions.DependencyInjection;
namespace Tryitter.Test;

public class UserTest : IClassFixture<WebApplicationFactory<program>>
{
    public HttpClient client;

    public UserTest(WebApplicationFactory<program> factory)
    {
        client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TryitterContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<TryitterContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTest");
                });
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<TryitterContext>())
                {
                    appContext.Database.EnsureCreated();
                    appContext.Database.EnsureDeleted();
                    appContext.Database.EnsureCreated();
                    appContext.Users.Add(new User { UserId = 1, Username = "Test", Name="Test", Password="Test", Email = "Test" });
                    appContext.SaveChanges();
                }
            });
        }).CreateClient();
    }

    [Theory(DisplayName = "POST /User deve retornar o user criado")]
    [MemberData(nameof(ShouldCreateAVideoData))]
    public async Task ShouldCreateAVideo(User userExpected)
    {
        var json = JsonConvert.SerializeObject(userExpected);
        var body = new StringContent(json, Encoding.UTF8, "application/json"); 
        var response =  await client.PostAsync("/User", body);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<User>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Name.Should().Be(userExpected.Name);
        result.Username.Should().Be(userExpected.Username);
        result.Email.Should().Be(userExpected.Email);
        result.Password.Should().Be(userExpected.Password);
        result.UserId.Should().Be(2);
    }

    public static readonly TheoryData<User> ShouldCreateAVideoData = new()
    {
        new User()
        {
            Username = "user",
            Name = "name",
            Email = "email@email.com",
            Password = "password123"
        }
    };

    [Theory(DisplayName = "GET /User deve retornar uma lista de users")]
    [MemberData(nameof(ShouldGetAllUsersData))]
    public async Task ShouldGetAllUsers(List<User> usersExpected)
    {
        var response =  await client.GetAsync("/User");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<User>>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(usersExpected);
    }

    public static readonly TheoryData<List<User>> ShouldGetAllUsersData = new()
    {
        new()
        {
            new User 
            { 
              UserId = 1,
              Username = "Test",
              Name="Test",
              Password="Test",
              Email = "Test" 
            }
        }
    };

    [Theory(DisplayName = "GET /User/{id} deve retornar um user")]
    [MemberData(nameof(ShouldGetUserbyIdData))]
    public async Task ShouldGetUserbyId(User userExpected)
    {
        var response =  await client.GetAsync("/User/1");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<User>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(userExpected);
    }

    public static readonly TheoryData<User> ShouldGetUserbyIdData = new()
    {
        new User 
        { 
            UserId = 1,
            Username = "Test",
            Name="Test",
            Password="Test",
            Email = "Test" 
        }
    };

    [Fact(DisplayName = "GET /User/{id} com id inválido deve retornar not found")]
    public async Task ShouldGetUserInvalid()
    {
        var response =  await client.GetAsync("/User/2");
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("User not found");
    }

}