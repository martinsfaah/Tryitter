namespace Tryitter.UseCases;

using BCrypt.Net;

using Tryitter.Models;
using Tryitter.Repositories;
using Tryitter.RequestHandlers;
using Tryitter.Services;
public class UserUseCase
{
  private readonly UserRepository _repository;
  public UserUseCase(UserRepository repository)
  {
    _repository = repository;
  }
  public async Task<string?> Auth(string Email, string Password)
  {
    var user = await _repository.GetByEmail(Email);
    
    if (user == null)
    {
      return null;
    }

    var isPasswordValid = BCrypt.Verify(Password, user.Password);

    if (!isPasswordValid)
    {
      return null;
    }

    var token = new AuthenticationService().GenerateToken(user);

    return token;
  }
  
  public User Create(User user)
  {
    var passwordHash = BCrypt.HashPassword(user.Password);

    user.Password = passwordHash;

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

  public User? Update(int id, UpdateRequest newUser)
  {
    var user = _repository.GetById(id);

    if (user is null)
    {
      return null;
    }

    user.Name = newUser.Name;
    user.Email = newUser.Email;

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