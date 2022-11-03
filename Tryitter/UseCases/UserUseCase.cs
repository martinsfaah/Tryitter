using Tryitter.Repositories;
using Tryitter.Services;
namespace Tryitter.UseCases;
public class UserUseCase
{
  private readonly IUserRepository _repository;
  public UserUseCase(IUserRepository repository)
  {
    _repository = repository;
  }
  public async Task<string?> Auth(string Email, string Password)
  {
    var user = await _repository.GetByEmail(Email);
    
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