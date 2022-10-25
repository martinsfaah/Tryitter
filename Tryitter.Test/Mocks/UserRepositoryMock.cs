using Microsoft.EntityFrameworkCore;
using System.Linq;

using Tryitter.Models;

namespace Tryitter.Repositories.Mock;

public class UserRepositoryMock : IUserRepository
{
  List<User> users = new (){
    new User() {
      UserId = 1,
      Username = "johndoe",
      Email = "johndoe@example.com",
      Name = "John Doe",
      Password = "123456"
    },
    new User() {
      UserId = 2,
      Username = "fulano",
      Email = "fulano@example.com",
      Name = "Fulano Silva",
      Password = "123456"
    },
    new User() {
      UserId = 3,
      Username = "beltrano",
      Email = "beltrano@example.com",
      Name = "Beltrano Silva",
      Password = "123456"
    },
  };


  public async Task<User?> GetByEmail(string Email)
  {
    return await users.AsQueryable().FirstOrDefaultAsync(user => user.Email == Email);
  }
}