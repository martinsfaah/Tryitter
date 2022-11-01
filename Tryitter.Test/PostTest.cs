using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Tryitter.Repositories;
using Tryitter.Models;
using Microsoft.Extensions.DependencyInjection;
namespace Tryitter.Test;

public class PostTest : IClassFixture<WebApplicationFactory<program>>
{
    public HttpClient client;

    public PostTest(WebApplicationFactory<program> factory)
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
                    options.UseInMemoryDatabase("MemoryTest");
                });
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<TryitterContext>())
                {
                    appContext.Database.EnsureCreated();
                    appContext.Database.EnsureDeleted();
                    appContext.Database.EnsureCreated();
                    appContext.Users.Add(new User { UserId = 1, Username = "Test", Name="Test", Password="Test", Email = "Test" });
                    appContext.Posts.Add(new Post { PostId = 1, Content = "Test", ImageUrl = "Test", UserId = 1 });
                    appContext.SaveChanges();
                }
            });
        }).CreateClient();
    }

    [Theory(DisplayName = "POST /Post deve retornar o post criado")]
    [MemberData(nameof(ShouldCreateAPostData))]
    public async Task ShouldCreateAPost(Post postExpected)
    {
        var json = JsonConvert.SerializeObject(postExpected);
        var body = new StringContent(json, Encoding.UTF8, "application/json"); 
        var response =  await client.PostAsync("/Post", body);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Post>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        result.Content.Should().Be(postExpected.Content);
        result.ImageUrl.Should().Be(postExpected.ImageUrl);
        result.UserId.Should().Be(postExpected.UserId);
        result.PostId.Should().Be(2);
    }

    public static readonly TheoryData<Post> ShouldCreateAPostData = new()
    {
        new Post()
        {
            Content = "Post Text",
            ImageUrl = "www.image.com",
            UserId = 1
        }
    };

    [Theory(DisplayName = "POST /Post com id inválido deve retornar not found")]
    [MemberData(nameof(ShouldCreateAPostInvalidData))]
    public async Task ShouldCreateAPostInvalid(Post postExpected)
    {
        var json = JsonConvert.SerializeObject(postExpected);
        var body = new StringContent(json, Encoding.UTF8, "application/json"); 
        var response =  await client.PostAsync("/Post", body);
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("User not found");
    }

    public static readonly TheoryData<Post> ShouldCreateAPostInvalidData = new()
    {
        new Post()
        {
            Content = "Post Text",
            ImageUrl = "www.image.com",
            UserId = 2
        }
    };


}