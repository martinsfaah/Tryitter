using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

using Tryitter.Models;
using Tryitter.Data;
using Tryitter.ViewModels;
using Tryitter.ViewModels.Post;
using Tryitter.ViewModels.User;

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
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword("Test");
                    
                    appContext.Database.EnsureCreated();
                    appContext.Database.EnsureDeleted();
                    appContext.Database.EnsureCreated();
                    var user = appContext.Users.Add(new User { Username = "Test", Name="Test", Password=passwordHash, Email = "Test", Role = "User",Module = "Test", Status = "Test" });
                    appContext.Posts.Add(new Post { Content = "Test", ImageUrl = "Imagem qualquer", ContentType = "image/png", User = user.Entity });
                    appContext.SaveChanges();

                }
            });
        }).CreateClient();

    }

    [Theory(DisplayName = "POST /Post deve retornar o post criado")]
    [MemberData(nameof(ShouldCreateAPostData))]
    public async Task ShouldCreateAPost(CreatePostViewModel post, ListPostViewModel postExpected)
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

        var json = JsonConvert.SerializeObject(post);
        var body = new StringContent(json, Encoding.UTF8, "application/json"); 
        var response =  await client.PostAsync("/Post", body);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ListPostViewModel>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        result.Content.Should().Be(postExpected.Content);
        result.ImageUrl.Should().BeEquivalentTo(postExpected.ImageUrl);
        result.User.UserId.Should().Be(postExpected.User.UserId);
        result.Id.Should().Be(2);
    }

    public static readonly TheoryData<CreatePostViewModel, ListPostViewModel> ShouldCreateAPostData = new()
    {
        {
            new CreatePostViewModel
            {
                Content = "Test",
                ImageUrl = "Imagem qualquer",
                ContentType = "image/png",
            },
            new ListPostViewModel
                { 
                Id = 1,
                Content = "Test",
                ImageUrl = "Imagem qualquer",
                ContentType = "image/png",
                User = new ShowUserViewModel {
                    UserId = 1,
                    Name = "Test",
                    Username = "Test"
                }
                }
        }
    };

    [Theory(DisplayName = "GET /Post deve retornar uma lista de posts")]
    [MemberData(nameof(ShouldGetAllPostsData))]
    public async Task ShouldGetAllPosts(List<ListPostViewModel> postsExpected)
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

        var response =  await client.GetAsync("/Post");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<ListPostViewModel>>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(postsExpected);
    }

    public static readonly TheoryData<List<ListPostViewModel>> ShouldGetAllPostsData = new()
    {
        new ()
        {
            new ListPostViewModel
            { 
              Id = 1,
              Content = "Test",
              ImageUrl = "Imagem qualquer",
              ContentType = "image/png",
              User = new ShowUserViewModel {
                UserId = 1,
                Name = "Test",
                Username = "Test"
              }
            }
        }
    };

    [Theory(DisplayName = "GET /Post/{id} deve retornar um post")]
    [MemberData(nameof(ShouldGetPostbyIdData))]
    public async Task ShouldGetPostbyId(ListPostViewModel postExpected)
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

        var response =  await client.GetAsync("/Post/1");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ListPostViewModel>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(postExpected);
    }

    public static readonly TheoryData<ListPostViewModel> ShouldGetPostbyIdData = new()
    {
        new ListPostViewModel
        { 
            Id = 1,
            Content = "Test",
            ImageUrl = "Imagem qualquer",
            ContentType = "image/png",
            User = new ShowUserViewModel {
            UserId = 1,
            Name = "Test",
            Username = "Test"
            }
        }
    };

    [Fact(DisplayName = "GET /Post/{id} com id inválido deve retornar not found")]
    public async Task ShouldGetPostInvalid()
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

        var response =  await client.GetAsync("/Post/2");
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("Post not found");
    }

    [Theory(DisplayName = "UPDATE /Post/{id} deve retornar um post")]
    [MemberData(nameof(ShouldUpdatePostData))]
    public async Task ShouldUpdatePost(ListPostViewModel postExpected)
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

        var json = JsonConvert.SerializeObject(postExpected);
        var body = new StringContent(json, Encoding.UTF8, "application/json"); 
        var response =  await client.PutAsync("/Post/1", body);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ListPostViewModel>(content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEquivalentTo(postExpected);
    }

    public static readonly TheoryData<ListPostViewModel> ShouldUpdatePostData = new()
    {
        new ListPostViewModel
        { 
            Id = 1,
            Content = "Test",
            ImageUrl = "Imagem qualquer",
            ContentType = "image/png",
            User = new ShowUserViewModel {
            UserId = 1,
            Name = "Test",
            Username = "Test"
            }
        }
    };

    [Theory(DisplayName = "UPDATE /Post/{id} com id inválido deve retornar not found")]
    [MemberData(nameof(ShouldUpdatePostInvalidData))]
    public async Task ShouldUpdatePostInvalid(ListPostViewModel postExpected)
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

        var json = JsonConvert.SerializeObject(postExpected);
        var body = new StringContent(json, Encoding.UTF8, "application/json");
        var response =  await client.PutAsync("/Post/2", body);
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("Post not found");
    }
    public static readonly TheoryData<ListPostViewModel> ShouldUpdatePostInvalidData = new()
    {
        new ListPostViewModel
        { 
            Id = 1,
            Content = "Test",
            ImageUrl = "Imagem qualquer",
            ContentType = "image/png",
            User = new ShowUserViewModel {
            UserId = 1,
            Name = "Test",
            Username = "Test"
            }
        }
    };

    [Fact(DisplayName = "DELETE /Post/{id} deve apagar um post")]
    public async Task ShouldDeletePostbyId()
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
        var user = new AuthenticateViewModel() {
            Email = "Test",
            Password = "Test"
        };
        var json = JsonConvert.SerializeObject(user);
        var body = new StringContent(json, Encoding.UTF8, "application/json");

        var token = await client.PostAsync("/Auth", body);
        var responseToken = await token.Content.ReadAsStringAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseToken);

        var response =  await client.DeleteAsync("/Post/2");
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        content.Should().BeEquivalentTo("Post not found");
    }
}