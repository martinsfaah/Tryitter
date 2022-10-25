using Tryitter.Repositories;
using Tryitter.Services;
namespace Tryitter.UseCases;
public class UserUseCase
{
  private readonly IUserRepository _repository;
  public UserUseCase(IUserRepository repository)
  {
    _repository = repository;
  }
  public async Task<string?> Auth(string Email, string Password)
  {
    var user = await _repository.GetByEmail(Email);
    
    if (user == null || user.Password != Password)
    {
      return null;
    }

    var token = new AuthenticationService().GenerateToken(user);

    return token;
  }
}