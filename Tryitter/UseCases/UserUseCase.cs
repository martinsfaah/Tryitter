using Tryitter.Repositories;
using Tryitter.Services;
using Tryitter.Models;
namespace Tryitter.UseCases;
public class UserUseCase
{
  private UserRepository _repository;
  public UserUseCase(UserRepository repository)
  {
    _repository = repository;
  }
  public string? Auth(string Email, string Password)
  {
    var user = _repository.GetByEmail(Email);
    
    if (user == null || user.Password != Password)
    {
      return null;
    }

    var token = new AuthenticationService().GenerateToken(user);

    return token;
  }
  
  public User Create(User user)
  {
    var created = _repository.Create(user);

    return created;
  }
  
  public List<User> GetAll()
  {
    var users = _repository.GetAll();

    return users;
  }
}