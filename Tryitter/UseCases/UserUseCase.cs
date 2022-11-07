namespace Tryitter.UseCases;

using BCrypt.Net;

using Tryitter.Models;
using Tryitter.Repositories;
using Tryitter.Services;
using Tryitter.ViewModels.User;

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
  
  public async Task<User> Create(CreateUserViewModel user)
  {
    var passwordHash = BCrypt.HashPassword(user.Password);

    user.Password = passwordHash;

    var newUser = new User() {
        Username = user.Username,
        Email = user.Email,
        Name = user.Name,
        Password = user.Password,
        Module = user.Module,
        Status = user.Status
    };

    var created = await _repository.Create(newUser);

    return created;
  }
  
  public async Task<List<ListUserViewModel>> GetAll()
  {
    var users = await _repository.GetAll();

    return users;
  }

  public async Task<ListUserWithPostsViewModel?> GetById(int id)
  {
    var user = await _repository.GetById(id);

    return user;
  }


  public async Task<List<ListUserViewModel>> GetByName(string name)
  {
    var user = await _repository.GetByName(name);

    return user;
  }

  public async Task<User?> Update(string id, UpdateUserViewModel userToUpdate)
  {
    var userId = Convert.ToInt32(id);
    var updated = await _repository.Update(userId, userToUpdate);

    return updated;
  }

  public async Task<User?> UpdateRole(string id, string role)
  {
    var userId = Convert.ToInt32(id);

    var updatedUser = await _repository.UpdateRole(userId, role);

    if (updatedUser is null)
    {
      return null;
    }

    return updatedUser;
  }
  public async Task<User?> Delete(string id)
  {
    var userId = Convert.ToInt32(id);
    
    var deletedUser = await _repository.Delete(userId);

    if (deletedUser is null)
    {
      return null;
    }

    return deletedUser;
  }
}
