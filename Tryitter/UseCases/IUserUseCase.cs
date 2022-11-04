using Tryitter.Repositories;
using Tryitter.Services;
using Tryitter.Models;

namespace Tryitter.UseCases;

public interface IUserUseCase
{
  string? Auth(string Email, string Password);
  Task<User> Create(User user);
  Task<List<User>> GetAll();
  Task<User?> GetById(int id);
  Task<List<User>> GetByName(string name);
  Task<User?> Update(int id, User newUser);
  Task<User?> Delete(int id);
}