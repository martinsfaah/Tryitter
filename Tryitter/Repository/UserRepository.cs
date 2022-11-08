using Microsoft.EntityFrameworkCore;

<<<<<<< HEAD
using Tryitter.Data;
using Tryitter.Models;
using Tryitter.ViewModels.Post;
using Tryitter.ViewModels.User;
=======
using Tryitter.Models;
>>>>>>> 89d6a0bc1ef7860dc44d94f4ee43b90dd9692f93

namespace Tryitter.Repositories;

public class UserRepository : IUserRepository
{
  private readonly TryitterContext _context;
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
  
  public async Task<List<ListUserViewModel>> GetAll()
  {
<<<<<<< HEAD
    var users = await _context.Users
      .AsNoTracking()
      .Select(x =>
        new ListUserViewModel()
        {
          Id = x.UserId,
          Username = x.Username,
          Name = x.Name,
          Email = x.Email,
          Role = x.Role,
          Module = x.Module,
          Status = x.Status
        }
      )
      .ToListAsync();
=======
    var users = await _context.Users.AsNoTracking().ToListAsync();
>>>>>>> 89d6a0bc1ef7860dc44d94f4ee43b90dd9692f93


    return users;
  }

  public async Task<ListUserWithPostsViewModel?> GetById(int id)
  {
<<<<<<< HEAD
    var user = await _context.Users
      .AsNoTracking()
      .Include(x => x.Posts)
      .Select(x =>
        new ListUserWithPostsViewModel()
        {
          Id = x.UserId,
          Username = x.Username,
          Name = x.Name,
          Email = x.Email,
          Role = x.Role,
          Module = x.Module,
          Status = x.Status,
          Posts = x.Posts.Select(y =>
            new ShowPostViewModel()
            {
              Id = y.PostId,
              Content = y.Content,
              ImageUrl = y.ImageUrl,
              ContentType = y.ContentType
            }
          )
        }
      )
      .FirstOrDefaultAsync(x => x.Id == id);
=======
    var user = await _context.Users.Include(x => x.Posts).FirstOrDefaultAsync(x => x.UserId == id);
>>>>>>> 89d6a0bc1ef7860dc44d94f4ee43b90dd9692f93

    return user;
  }

  public async Task<List<ListUserViewModel>> GetByName(string name)
  {
<<<<<<< HEAD
    var user = await _context.Users
      .AsNoTracking()
      .Where(x => x.Name.Contains(name))
      .Include(x => x.Posts)
      .Select(x =>
        new ListUserViewModel()
        {
          Id = x.UserId,
          Username = x.Username,
          Name = x.Name,
          Email = x.Email,
          Role = x.Role,
          Module = x.Module,
          Status = x.Status
        }
      )
      .ToListAsync();
=======
    var user = await _context.Users.AsNoTracking().Where(x => x.Name.Contains(name)).Include(x => x.Posts).ToListAsync();
>>>>>>> 89d6a0bc1ef7860dc44d94f4ee43b90dd9692f93

    return user;
  }

  public async Task<User?> Update(int userId, UpdateUserViewModel userToUpdate)
  {
    var userFound = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
    if (userFound is null)
    {
<<<<<<< HEAD
      return null;
=======
      for (int i = 0; i < user.Posts.Count; i++)
      {
        _context.Remove(user.Posts.ToList()[i]); // ! Pode resolver com map
      }
>>>>>>> 89d6a0bc1ef7860dc44d94f4ee43b90dd9692f93
    }

<<<<<<< HEAD
    userFound.Name = userToUpdate.Name;
    userFound.Email = userToUpdate.Email;
    userFound.Module = userToUpdate.Module;
    userFound.Status = userToUpdate.Status;

    _context.Update(userFound);
    await _context.SaveChangesAsync();

    return userFound;
  }
  
  public async Task<User?> UpdateRole(int userId, string role)
  {
    var userFound = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
    if (userFound is null)
    {
      return null;
    }

    userFound.Role = role;

    _context.Update(userFound);
    await _context.SaveChangesAsync();

    return userFound;
  }

  public async Task<User?> Delete(int userId)
  {
    var userFound = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

    if (userFound is null)
    {
      return null;
    }

    _context.Remove(userFound);
    await _context.SaveChangesAsync();
    return userFound;
  }

  public async Task<User?> GetByEmail(string Email)
  {
    return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == Email);
  }
=======
  public async Task<User?> GetByEmail(string Email)
  {
    return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == Email);
  }
>>>>>>> 89d6a0bc1ef7860dc44d94f4ee43b90dd9692f93

}