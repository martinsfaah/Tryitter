using Tryitter.Models;

namespace Tryitter.Repositories;

public interface IUserRepository
{
  User Create(User user);
  List<User> GetAll();
  User? GetById(int id);
  List<User> GetByName(string name);
  User Update(User user);
  User Delete(User user);
  User? GetByEmail(string Email);
}
