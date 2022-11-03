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
                    appContext.Users.Add(new User { UserId = 1, Username = "Test", Name="Test", Password="Test", Email = "Test", Modulo = "Test", Status = "Test" });
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

    [Theory(DisplayName = "GET /Post deve retornar uma lista de posts")]
    [MemberData(nameof(ShouldGetAllPostsData))]
    public async Task ShouldGetAllPosts(List<Post> postsExpected)
    {
        var response =  await client.GetAsync("/Post");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<Post>>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(postsExpected);
    }

    public static readonly TheoryData<List<Post>> ShouldGetAllPostsData = new()
    {
        new()
        {
            new Post 
            { 
              PostId = 1,
              Content = "Test",
              ImageUrl ="Test",
              UserId = 1
            }
        }
    };

    [Theory(DisplayName = "GET /Post/{id} deve retornar um post")]
    [MemberData(nameof(ShouldGetPostbyIdData))]
    public async Task ShouldGetPostbyId(Post postExpected)
    {
        var response =  await client.GetAsync("/Post/1");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Post>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(postExpected);
    }

    public static readonly TheoryData<Post> ShouldGetPostbyIdData = new()
    {
        new Post 
        { 
            PostId = 1,
            Content = "Test",
            ImageUrl ="Test",
            UserId = 1 
        }
    };

    [Fact(DisplayName = "GET /Post/{id} com id inválido deve retornar not found")]
    public async Task ShouldGetPostInvalid()
    {
        var response =  await client.GetAsync("/Post/2");
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("Post not found");
    }

    [Theory(DisplayName = "UPDATE /Post/{id} deve retornar um post")]
    [MemberData(nameof(ShouldUpdatePostData))]
    public async Task ShouldUpdatePost(Post postExpected)
    {
        var json = JsonConvert.SerializeObject(postExpected);
        var body = new StringContent(json, Encoding.UTF8, "application/json"); 
        var response =  await client.PutAsync("/Post/1", body);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Post>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(postExpected);
    }

    public static readonly TheoryData<Post> ShouldUpdatePostData = new()
    {
        new Post 
        { 
            PostId = 1,
            Content = "newTest",
            ImageUrl ="newTest",
            UserId = 1
        }
    };

    [Theory(DisplayName = "UPDATE /Post/{id} com id inválido deve retornar not found")]
    [MemberData(nameof(ShouldUpdatePostInvalidData))]
    public async Task ShouldUpdatePostInvalid(Post postExpected)
    {
        var json = JsonConvert.SerializeObject(postExpected);
        var body = new StringContent(json, Encoding.UTF8, "application/json");
        var response =  await client.PutAsync("/Post/2", body);
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("Post not found");
    }
    public static readonly TheoryData<Post> ShouldUpdatePostInvalidData = new()
    {
        new Post 
        { 
            PostId = 1,
            Content = "newTest",
            ImageUrl ="newTest",
            UserId = 1 
        }
    };

    [Fact(DisplayName = "DELETE /Post/{id} deve apagar um post")]
    public async Task ShouldDeletePostbyId()
    {
        var resp =  await client.DeleteAsync("/Post/1");
        resp.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        var response =  await client.GetAsync("/Post/1");
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("Post not found");
    }

    [Fact(DisplayName = "DELETE /Post/{id} com id inválido deve retornar not found")]
    public async Task ShouldDeletePostInvalid()
    {
        var response =  await client.DeleteAsync("/Post/2");
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("Post not found");
    }
}