using Tryitter.Repositories;
using Tryitter.Services;
using Tryitter.Models;
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
  
  public Post? Create(Post post)
  {
    var user = _userRepository.GetById(post.UserId);

    if (user is null)
    {
      return null;
    }
    
    var created = _repository.Create(post);

    return created;
  }
  
  public List<Post> GetAll()
  {
    var posts = _repository.GetAll();

    return posts;
  }


}