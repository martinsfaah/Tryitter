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
  
  public async Task<Post?> Create(Postrequest post)
  {
    var user = _userRepository.GetById(post.UserId);

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
    
    var created = _repository.Create(newPost);

    return created;
  }
  
  public List<Post> GetAll()
  {
    var posts = _repository.GetAll();

    return posts;
  }

  public Post? GetById(int id)
  {
    var post = _repository.GetById(id);

    return post;
  }

  public Post? Update(int id, Post newPost)
  {
    var post = _repository.GetById(id);

    if (post is null)
    {
      return null;
    }

    post.UserId = newPost.UserId;
    post.Content = newPost.Content;

    var updated = _repository.Update(post);

    return updated;
  }

  public Post? Delete(int id)
  {
    var post = _repository.GetById(id);

    if (post is null)
    {
      return null;
    }
    
    var deletedPost = _repository.Delete(post);

    return deletedPost;
  }
}