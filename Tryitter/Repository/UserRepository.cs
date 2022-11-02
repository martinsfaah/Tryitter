using Tryitter.Models;
using Microsoft.EntityFrameworkCore;

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
    var users = _context.Users.ToList();

    return users;
  }

  public User? GetById(int id)
  {
    var user = _context.Users.Where(x => x.UserId == id).FirstOrDefault();

    return user;
  }

  public List<User> GetByName(string name)
  {
    var user = _context.Users.Where(x => x.Name.Contains(name)).ToList();

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

  public User? GetByEmail(string Email)
  {
    var userFound = _context.Users?.FirstOrDefault(user => user.Email == Email);

    return userFound;
  }
}