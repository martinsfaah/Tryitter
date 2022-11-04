namespace Tryitter.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

using Tryitter.UseCases;
using Tryitter.RequestHandlers;
using Tryitter.Models;
using Tryitter.Services;

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

  [HttpPost]
  [AllowAnonymous]
  public ActionResult<User> Create([FromBody] User user)
  {
    try
    {
      var created = _userUseCase.Create(user);

      return CreatedAtAction("GetById", new { id = created.UserId }, created);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpGet]
  [Authorize]
  public ActionResult<User> GetAll()
  {
    try
    {
      var users = _userUseCase.GetAll();

      return Ok(users);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpGet("{id}")]
  [Authorize]
  public ActionResult<User> GetById([FromRoute] string id)
  {
    try
    {
      int IdNumber = Convert.ToInt32(id);
      
      var user = _userUseCase.GetById(IdNumber);

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

  [HttpPut("{id}")]
  [Authorize]
  public ActionResult<User> Update([FromRoute] string id, [FromBody] UpdateRequest newUser)
  {
    try
    {
      var instance = new AuthorizationService();
      var isValidUser = instance.VerifyIdentity(id, User);
      if (!isValidUser)
      {
        return Unauthorized();
      }


      int IdNumber = Convert.ToInt32(id);

      var user = _userUseCase.Update(IdNumber, newUser);

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
  [Authorize]
  public ActionResult Delete([FromRoute] string id)
  {
    try
    {
      var isValidUser = new AuthorizationService().VerifyIdentity(id, User);
      if (!isValidUser)
      {
        return Unauthorized();
      }

      int IdNumber = Convert.ToInt32(id);
      
      var user = _userUseCase.Delete(IdNumber);

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