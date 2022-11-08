using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Tryitter.Data;
using Tryitter.Models;
using Microsoft.Extensions.DependencyInjection;
using Tryitter.ViewModels.User;
using Tryitter.ViewModels.Post;
using Tryitter.ViewModels;
using System.Net.Http.Headers;

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
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword("Test");

                    appContext.Database.EnsureCreated();
                    appContext.Database.EnsureDeleted();
                    appContext.Database.EnsureCreated();
                    appContext.Users.Add(new User { Username = "Test", Name="Test", Password=passwordHash, Email = "Test", Role = "Admin",Module = "Test", Status = "Test" });
                    appContext.SaveChanges();
                }
            });
        }).CreateClient();
    }

    [Theory(DisplayName = "POST /User deve retornar o user criado")]
    [MemberData(nameof(ShouldCreateAUserData))]
    public async Task ShouldCreateAUser(User userExpected)
    {
        var json = JsonConvert.SerializeObject(userExpected);
        var body = new StringContent(json, Encoding.UTF8, "application/json"); 
        var response =  await client.PostAsync("/User", body);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ListUserViewModel>(content);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        result.Name.Should().Be(userExpected.Name);
        result.Username.Should().Be(userExpected.Username);
        result.Email.Should().Be(userExpected.Email);
        result.Module.Should().Be(userExpected.Module);
        result.Status.Should().Be(userExpected.Status);
        result.Id.Should().Be(2);
    }

    public static readonly TheoryData<User> ShouldCreateAUserData = new()
    {
        new User()
        {
            UserId = 2,
            Username = "user",
            Email = "email@email.com",
            Name = "name",
            Password = "Password12!",
            Role = "User",
            Module = "Front-End",
            Status = "Ativo"
        }
    };

    [Theory(DisplayName = "GET /User deve retornar uma lista de users")]
    [MemberData(nameof(ShouldGetAllUsersData))]
    public async Task ShouldGetAllUsers(List<ListUserViewModel> usersExpected)
    {
        var user = new AuthenticateViewModel() {
            Email = "Test",
            Password = "Test"
        };
        var json = JsonConvert.SerializeObject(user);
        var body = new StringContent(json, Encoding.UTF8, "application/json");

        var token = await client.PostAsync("/Auth", body);
        var responseToken = await token.Content.ReadAsStringAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseToken);

        var response =  await client.GetAsync("/User");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<ListUserViewModel>>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(usersExpected);
    }

    public static readonly TheoryData<List<ListUserViewModel>> ShouldGetAllUsersData = new()
    {
        new()
        {
            new ListUserViewModel 
            { 
              Id = 1,
              Username = "Test",
              Email = "Test",
              Name="Test",
              Role = "Admin",
              Module = "Test",
              Status = "Test"
            }
        }
    };

    [Theory(DisplayName = "GET /User/{id} deve retornar um user")]
    [MemberData(nameof(ShouldGetUserbyIdData))]
    public async Task ShouldGetUserbyId(ListUserWithPostsViewModel userExpected)
    {
        var user = new AuthenticateViewModel() {
            Email = "Test",
            Password = "Test"
        };
        var json = JsonConvert.SerializeObject(user);
        var body = new StringContent(json, Encoding.UTF8, "application/json");

        var token = await client.PostAsync("/Auth", body);
        var responseToken = await token.Content.ReadAsStringAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseToken);

        var response =  await client.GetAsync("/User/1");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ListUserWithPostsViewModel>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(userExpected);
    }

    public static readonly TheoryData<ListUserWithPostsViewModel> ShouldGetUserbyIdData = new()
    {
        new ListUserWithPostsViewModel 
        { 
            Id = 1,
            Username = "Test",
            Email = "Test",
            Name= "Test",
            Role = "Admin",
            Module = "Test",
            Status = "Test",
            Posts = new List<ShowPostViewModel>()
            {
                
            }
        }
    };

    [Fact(DisplayName = "GET /User/{id} com id inválido deve retornar not found")]
    public async Task ShouldGetUserInvalid()
    {
        var user = new AuthenticateViewModel() {
            Email = "Test",
            Password = "Test"
        };
        var json = JsonConvert.SerializeObject(user);
        var body = new StringContent(json, Encoding.UTF8, "application/json");

        var token = await client.PostAsync("/Auth", body);
        var responseToken = await token.Content.ReadAsStringAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseToken);

        var response =  await client.GetAsync("/User/2");
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("User not found");
    }

    [Theory(DisplayName = "GET /User/Name/{name} deve retornar uma lista de users")]
    [MemberData(nameof(ShouldGetUserbyNameData))]
    public async Task ShouldGetUserbyName(List<ListUserViewModel> usersExpected)
    {
        var user = new AuthenticateViewModel() {
            Email = "Test",
            Password = "Test"
        };
        var json = JsonConvert.SerializeObject(user);
        var body = new StringContent(json, Encoding.UTF8, "application/json");

        var token = await client.PostAsync("/Auth", body);
        var responseToken = await token.Content.ReadAsStringAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseToken);

        var response =  await client.GetAsync("/User/Name/Test");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<ListUserViewModel>>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(usersExpected);
    }

    public static readonly TheoryData<List<ListUserViewModel>> ShouldGetUserbyNameData = new()
    {
        new()
        {
            new ListUserViewModel 
            { 
                Id = 1,
                Username = "Test",
                Email = "Test",
                Name= "Test",
                Role = "Admin",
                Module = "Test",
                Status = "Test",
            }
        }
    };

    [Theory(DisplayName = "UPDATE /User/{id} deve retornar um user")]
    [MemberData(nameof(ShouldUpdateUserData))]
    public async Task ShouldUpdateUser(UpdateUserViewModel userSent, ListUserViewModel userExpected)
    {
        var user = new AuthenticateViewModel() {
            Email = "Test",
            Password = "Test"
        };
        var jsonToken = JsonConvert.SerializeObject(user);
        var bodyToken = new StringContent(jsonToken, Encoding.UTF8, "application/json");

        var token = await client.PostAsync("/Auth", bodyToken);
        var responseToken = await token.Content.ReadAsStringAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseToken);

        var json = JsonConvert.SerializeObject(userSent);
        var body = new StringContent(json, Encoding.UTF8, "application/json"); 
        var response =  await client.PutAsync("/User/1", body);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ListUserViewModel>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(userExpected);
    }

    public static readonly TheoryData<UpdateUserViewModel, ListUserViewModel> ShouldUpdateUserData = new()
    {
        {
            new UpdateUserViewModel
            {
                Email = "Test",
                Name = "Test",
                Module = "newTest",
                Status = "newTest"
            },
            new ListUserViewModel 
            { 
                Id = 1,
                Username = "Test",
                Email = "Test",
                Name="Test",
                Role = "Admin",
                Module = "newTest",
                Status = "newTest"
            }
        }
    };

    [Theory(DisplayName = "UPDATE /User/{id} com id inválido deve retornar not found")]
    [MemberData(nameof(ShouldUpdateUserInvalidData))]
    public async Task ShouldUpdateUserInvalid( UpdateUserViewModel userExpected)
    {
        var user = new AuthenticateViewModel() {
            Email = "Test",
            Password = "Test"
        };
        var jsonToken = JsonConvert.SerializeObject(user);
        var bodyToken = new StringContent(jsonToken, Encoding.UTF8, "application/json");

        var token = await client.PostAsync("/Auth", bodyToken);
        var responseToken = await token.Content.ReadAsStringAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseToken);

        var json = JsonConvert.SerializeObject(userExpected);
        var body = new StringContent(json, Encoding.UTF8, "application/json");
        var response =  await client.PutAsync("/User/2", body);
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("User not found");
    }
    public static readonly TheoryData<UpdateUserViewModel> ShouldUpdateUserInvalidData = new()
    {
        new UpdateUserViewModel 
        { 
            Email = "newTest",
            Name="newTest",
            Module = "newTest",
            Status = "newTest"
        }
    };

    [Fact(DisplayName = "DELETE /User/{id} deve apagar um user")]
    public async Task ShouldDeleteUserbyId()
    {
        var user = new AuthenticateViewModel() {
            Email = "Test",
            Password = "Test"
        };
        var json = JsonConvert.SerializeObject(user);
        var body = new StringContent(json, Encoding.UTF8, "application/json");

        var token = await client.PostAsync("/Auth", body);
        var responseToken = await token.Content.ReadAsStringAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseToken);

        var resp =  await client.DeleteAsync("/User/1");
        resp.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        var response =  await client.GetAsync("/User/1");
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("User not found");
    }

    [Fact(DisplayName = "DELETE /User/{id} com id inválido deve retornar not found")]
    public async Task ShouldDeleteUserInvalid()
    {
        var user = new AuthenticateViewModel() {
            Email = "Test",
            Password = "Test"
        };
        var json = JsonConvert.SerializeObject(user);
        var body = new StringContent(json, Encoding.UTF8, "application/json");

        var token = await client.PostAsync("/Auth", body);
        var responseToken = await token.Content.ReadAsStringAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseToken);

        var response =  await client.DeleteAsync("/User/2");
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("User not found");
    }
}