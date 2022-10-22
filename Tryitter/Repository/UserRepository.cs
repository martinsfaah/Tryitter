using Tryitter.Models;

namespace Tryitter.Repositories;

public class UserRepository
{
  private TryitterContext _context;
  public UserRepository(TryitterContext context)
  {
    _context = context;
  }

  public User Create(User user)
  {
    _context.Users.Add(user);
    _context.SaveChanges();
    return user;
  }

  public User? GetByEmail(string Email)
  {
    var userFound = _context.Users?.FirstOrDefault(user => user.Email == Email);

    return userFound;
  }
}