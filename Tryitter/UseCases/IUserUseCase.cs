using Tryitter.Repositories;
using Tryitter.Services;
using Tryitter.Models;

namespace Tryitter.UseCases;

public interface IUserUseCase
{
  string? Auth(string Email, string Password);
  User Create(User user);
  List<User> GetAll();
  User? GetById(int id);
  List<User> GetByName(string name);
  User? Update(int id, User newUser);
  User? Delete(int id);
}