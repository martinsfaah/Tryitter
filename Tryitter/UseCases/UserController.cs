using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


using Tryitter.UseCases;
using Tryitter.RequestHandlers;
namespace Tryitter.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly UserUseCase _userUseCase;
  public UserController(UserUseCase userUseCase)
  {
    _userUseCase = userUseCase;
  }

  [HttpPost("/Auth")]
  [AllowAnonymous]
  public async Task<ActionResult<string>> Authenticate([FromBody] AuthenticateRequest user)
  {
    try
    {
      var token = await _userUseCase.Auth(user.Email, user.Password);

      if (token == null)
      { 
        return NotFound("User not found");
      }

      return Ok(token);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }
}