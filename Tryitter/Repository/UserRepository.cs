using Microsoft.EntityFrameworkCore;

using Tryitter.Models;

namespace Tryitter.Repositories;

public class UserRepository : IUserRepository
{
  private TryitterContext _context;
  public UserRepository(TryitterContext context)
  {
    _context = context;
  }

  // public User Create(User user)
  // {}

  public async Task<User?> GetByEmail(string Email)
  {
    return await _context.Users.FirstOrDefaultAsync(user => user.Email == Email);
  }
}