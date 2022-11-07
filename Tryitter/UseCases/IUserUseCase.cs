using Tryitter.Models;
using Tryitter.ViewModels.User;

namespace Tryitter.UseCases;

public interface IUserUseCase
{
  Task<string?> Auth(string Email, string Password);
  Task<User> Create(CreateUserViewModel user);
  Task<List<ListUserViewModel>> GetAll();
  Task<ListUserWithPostsViewModel?> GetById(int id);
  Task<List<ListUserViewModel>> GetByName(string name);
  Task<User?> Update(string id, UpdateUserViewModel newUser);
  Task<User?> UpdateRole(string id, string role);
  Task<User?> Delete(string id);
}