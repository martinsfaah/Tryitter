using Tryitter.Models;

namespace Tryitter.Repositories;

public interface IUserRepository
{
  Task<User> Create(User user);
  Task<List<User>> GetAll();
  Task<User?> GetById(int id);
  Task<List<User>> GetByName(string name);
  Task<User> Update(User user);
  Task<User> Delete(User user);
  User? GetByEmail(string Email);
}
