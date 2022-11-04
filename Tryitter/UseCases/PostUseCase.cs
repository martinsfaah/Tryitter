using Tryitter.Repositories;
using Tryitter.Services;
using Tryitter.Models;
using Tryitter.RequestHandlers;
namespace Tryitter.UseCases;
public class PostUseCase : IPostUseCase
{
  private IPostRepository _repository;
  private IUserRepository _userRepository;
  public PostUseCase(IPostRepository repository, IUserRepository userRepository)
  {
    _repository = repository;
    _userRepository = userRepository;
  }
  
  public async Task<Post?> Create(PostCreateRequest post)
  {
    var user = await _userRepository.GetById(post.UserId);

    if (user is null)
    {
      return null;
    }

    List<byte[]> data = new();
      if (post.ImageUrl is not null)
      {
        if (post.ImageUrl.Length > 0)
        {
          using (var stream = new MemoryStream())
          {
            await post.ImageUrl.CopyToAsync(stream);

            data.Add(stream.ToArray());
          }

        }
      }

    var newPost = new Post() { Content = post.Content, ImageUrl = data[0], ContentType = post.ImageUrl.ContentType, UserId = post.UserId, User = user };
    
    var created = await _repository.Create(newPost);

    return created;
  }
  
  public async Task<List<Post>> GetAll()
  {
    var posts = await _repository.GetAll();

    return posts;
  }

  public async Task<Post?> GetById(int id)
  {
    var post = await _repository.GetById(id);

    return post;
  }

  public async Task<Post?> Update(int id, PostUpdateRequest newPost)
  {
    var post = await _repository.GetById(id);

    if (post is null)
    {
      return null;
    }

    post.UserId = newPost.UserId;
    post.Content = newPost.Content;

    var updated = await _repository.Update(post);

    return updated;
  }

  public async Task<Post?> Delete(int id)
  {
    var post = await _repository.GetById(id);

    if (post is null)
    {
      return null;
    }
    
    var deletedPost = await _repository.Delete(post);

    return deletedPost;
  }
}