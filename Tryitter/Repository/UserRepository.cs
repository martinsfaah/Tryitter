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

  public async Task<User> Create(User user)
  {
    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();

    return user;
  }
  
  public async Task<List<User>> GetAll()
  {
    var users = await _context.Users.ToListAsync();

    return users;
  }

  public async Task<User?> GetById(int id)
  {
    var user = await _context.Users.Where(x => x.UserId == id).Include(x => x.Posts).FirstOrDefaultAsync();

    return user;
  }

  public async Task<List<User>> GetByName(string name)
  {
    var user = await _context.Users.Where(x => x.Name.Contains(name)).Include(x => x.Posts).ToListAsync();

    return user;
  }

  public async Task<User> Update(User user)
  {
    _context.Update(user);
    await _context.SaveChangesAsync();

    return user;
  }
  
  public async Task<User> Delete(User user)
  {
    if (user.Posts is not null)
    {
      for (int i = 0; i < user.Posts.Count; i++)
      {
        _context.Remove(user.Posts.ToList()[i]);
      }
    }
    _context.Remove(user);
    await _context.SaveChangesAsync();
    return user;
  }

  public User? GetByEmail(string Email)
  {
    var userFound = _context.Users?.FirstOrDefault(user => user.Email == Email);

    return userFound;
  }
}