using Tryitter.Repositories;
using Tryitter.Services;
using Tryitter.Models;
using Tryitter.RequestHandlers;
namespace Tryitter.UseCases;
public class PostUseCase : IPostUseCase
{
  private readonly IPostRepository _repository;
  private readonly IUserRepository _userRepository;
  public PostUseCase(IPostRepository repository, IUserRepository userRepository)
  {
    _repository = repository;
    _userRepository = userRepository;
  }
  
  public async Task<Post?> Create(PostCreateRequest post, string userId)
  {
    // ? Virá convertido do frontend
    // List<byte[]> data = new(); // TODO Se der tempo verificar sobre cloud storage
    //   if (post.ImageUrl is not null)
    //   {
    //     if (post.ImageUrl.Length > 0)
    //     {
    //       using (var stream = new MemoryStream())
    //       {
    //         await post.ImageUrl.CopyToAsync(stream);

    //         data.Add(stream.ToArray());
    //       }

    //     }
    //   }
    // ? ------------------------------
    var newPost = new Post()
      {
        Content = post.Content,
        ImageUrl = post.ImageUrl,
        UserId = Convert.ToInt32(userId),
      };
    
    var created = await _repository.Create(newPost);

    return created;
  }
  
  public async Task<List<Post>> GetAll()
  {
    var posts = await _repository.GetAll();

    return posts;
  }

  public async Task<Post?> GetById(string id)
  {
    int postId = Convert.ToInt32(id);
    var post = await _repository.GetById(postId);

    return post;
  }

  public async Task<Post?> Update(string id, string content)
  {
    int postId = Convert.ToInt32(id);

    var post = await _repository.GetById(postId);
    if (post is null)
    {
      return null;
    }

    post.Content = content;
    var updated = await _repository.Update(post);

    return updated;
  }

  public async Task Delete(Post post)
  {    
    await _repository.Delete(post);
  }
}