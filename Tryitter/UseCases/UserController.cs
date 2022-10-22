using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


using Tryitter.UseCases;
using Tryitter.Models;
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

  [HttpPost("/auth")]
  [AllowAnonymous]
  public ActionResult<string> Authenticate([FromBody] AuthenticateRequest user)
  {
    try
    {
      var token = _userUseCase.Auth(user.Email, user.Password);

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

  [HttpPost]
  [AllowAnonymous]
  public ActionResult<User> Create([FromBody] User user)
  {
    try
    {
      var created = _userUseCase.Create(user);

      return Ok(created);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

}