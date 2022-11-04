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
  private readonly IUserUseCase _userUseCase;
  public UserController(IUserUseCase userUseCase)
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
  public async Task<ActionResult<User>> Create([FromBody] User user)
  {
    try
    {
      var created = await _userUseCase.Create(user);

      return CreatedAtAction("GetById", new { id = created.UserId }, created);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult<List<User>>> GetAll()
  {
    try
    {
      var users = await _userUseCase.GetAll();

      return Ok(users);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<User>> GetById([FromRoute] string id)
  {
    try
    {
      int IdNumber = Convert.ToInt32(id);
      
      var user = await _userUseCase.GetById(IdNumber);

      if (user is null)
      {
        return NotFound("User not found");
      }

      return Ok(user);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpGet("Name/{name}")]
  [AllowAnonymous]
  public async Task<ActionResult<List<User>>> GetByName([FromRoute] string name)
  {
    try
    {
      var user = await _userUseCase.GetByName(name);

      return Ok(user);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpPut("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<User>> Update([FromRoute] string id, [FromBody] User newUser)
  {
    try
    {
      int IdNumber = Convert.ToInt32(id);
      
      var user = await _userUseCase.Update(IdNumber, newUser);

      if (user is null)
      {
        return NotFound("User not found");
      }

      return Ok(user);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpDelete("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult> Delete([FromRoute] string id)
  {
    try
    {
      int IdNumber = Convert.ToInt32(id);
      
      var user = await _userUseCase.Delete(IdNumber);

      if (user is null)
      {
        return NotFound("User not found");
      }

      return NoContent();
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }
}