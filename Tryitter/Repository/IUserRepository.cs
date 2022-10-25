using Tryitter.Models;

namespace Tryitter.Repositories;
public interface IUserRepository {
  public Task<User?> GetByEmail(string email);
}