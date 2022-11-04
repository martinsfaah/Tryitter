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
  
  public async Task<User> Create(User user)
  {
    var created = await _repository.Create(user);

    return created;
  }
  
  public async Task<List<User>> GetAll()
  {
    var users = await _repository.GetAll();

    return users;
  }

  public async Task<User?> GetById(int id)
  {
    var user = await _repository.GetById(id);

    return user;
  }

  public async Task<List<User>> GetByName(string name)
  {
    var user = await _repository.GetByName(name);

    return user;
  }

  public async Task<User?> Update(int id, User newUser)
  {
    var user = await _repository.GetById(id);

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

    var updated = await _repository.Update(user);

    return updated;
  }

  public async Task<User?> Delete(int id)
  {
    var user = await _repository.GetById(id);

    if (user is null)
    {
      return null;
    }
    
    var deletedUser = await _repository.Delete(user);

    return deletedUser;
  }
}