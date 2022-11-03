using Microsoft.EntityFrameworkCore;

using Tryitter.Models;
using System.Data.Entity;

namespace Tryitter.Repositories;

public class UserRepository : IUserRepository
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
  
  public List<User> GetAll()
  {
    var users = _context.Users.Include(x => x.Posts).ToList();

    return users;
  }

  public User? GetById(int id)
  {
    var user = _context.Users.Where(x => x.UserId == id).Include(x => x.Posts).FirstOrDefault();

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
    return await _context.Users.FirstOrDefaultAsync(user => user.Email == Email);
  }
}