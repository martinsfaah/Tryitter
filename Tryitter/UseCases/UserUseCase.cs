namespace Tryitter.UseCases;

using BCrypt.Net;

using Tryitter.Models;
using Tryitter.Repositories;
using Tryitter.RequestHandlers;
using Tryitter.Services;

public class UserUseCase : IUserUseCase
{
  private readonly IUserRepository _repository;
  public UserUseCase(IUserRepository repository)

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
  
  public async Task<User> Create(User user)
  {
    var passwordHash = BCrypt.HashPassword(user.Password);

    user.Password = passwordHash;

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

  public async Task<User?> Update(int id, UpdateRequest newUser)
  {
    var user = await _repository.GetById(id);

    if (user is null)
    {
      return null;
    }

    user.Name = newUser.Name!;
    user.Email = newUser.Email!;
    user.Module = newUser.Module!;
    user.Status = newUser.Status!;

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
