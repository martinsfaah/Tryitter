using Microsoft.EntityFrameworkCore;

using Tryitter.Models;

namespace Tryitter.Repositories;

public class UserRepository : IUserRepository
{
  private readonly TryitterContext _context;
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
  
  public List<User> GetAll()
  {
    var users = _context.Users.AsNoTracking().Include(x => x.Posts).ToList();

    return users;
  }

  public User? GetById(int id)
  {
    var user = _context.Users.AsNoTracking().Include(x => x.Posts).FirstOrDefault(x => x.UserId == id);

    return user;
  }

  public User Update(User user)
  {
    _context.Update(user);
    _context.SaveChanges();

    return user;
  }
  
  public User Delete(User user)
  {
    if (user.Posts is not null)
    {
      for (int i = 0; i < user.Posts.Count; i++)
      {
        _context.Remove(user.Posts.ToList()[i]);
      }
    }
    _context.Remove(user);
    _context.SaveChanges();
    return user;
  }

  public async Task<User?> GetByEmail(string Email)
  {
    return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == Email);
  }
}