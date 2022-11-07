using Tryitter.Models;
using Tryitter.ViewModels.User;

namespace Tryitter.Repositories;

public interface IUserRepository
{
  Task<User> Create(User user);
  Task<List<ListUserViewModel>> GetAll();
  Task<ListUserWithPostsViewModel?> GetById(int id);
  Task<List<ListUserViewModel>> GetByName(string name);
  Task<User> Update(int userId, UpdateUserViewModel userToUpdate);
  Task<User?> UpdateRole(int userId, string role);
  Task<User?> Delete(int userId);
  Task<User?> GetByEmail(string Email);
}

