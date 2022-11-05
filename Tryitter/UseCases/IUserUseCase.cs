using Tryitter.Models;
using Tryitter.RequestHandlers;

namespace Tryitter.UseCases;

public interface IUserUseCase
{
  Task<string?> Auth(string Email, string Password);
  Task<User> Create(User user);
  Task<List<User>> GetAll();
  Task<User?> GetById(int id);
  Task<List<User>> GetByName(string name);
  Task<User?> Update(int id, UpdateRequest newUser);
  Task<User?> Delete(int id);
}