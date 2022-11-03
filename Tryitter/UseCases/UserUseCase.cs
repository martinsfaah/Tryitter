using Tryitter.Repositories;
using Tryitter.Services;
using Tryitter.Models;
namespace Tryitter.UseCases;
public class UserUseCase : IUserUseCase
{
  private IUserRepository _repository;
  public UserUseCase(IUserRepository repository)
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

  public User? GetById(int id)
  {
    var user = _repository.GetById(id);

    return user;
  }

  public List<User> GetByName(string name)
  {
    var user = _repository.GetByName(name);

    return user;
  }

  public User? Update(int id, User newUser)
  {
    var user = _repository.GetById(id);

    if (user is null)
    {
      return null;
    }

    user.Name = newUser.Name;
    user.Username = newUser.Username;
    user.Email = newUser.Email;
    user.Password = newUser.Password;
    user.Modulo = newUser.Modulo;
    user.Status = newUser.Status;

    var updated = _repository.Update(user);

    return updated;
  }

  public User? Delete(int id)
  {
    var user = _repository.GetById(id);

    if (user is null)
    {
      return null;
    }
    
    var deletedUser = _repository.Delete(user);

    return deletedUser;
  }
}