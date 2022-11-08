namespace Tryitter.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Tryitter.UseCases;
using Tryitter.ViewModels;
using Tryitter.ViewModels.User;
using Tryitter.Models;
using Tryitter.Services;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly IUserUseCase _userUseCase;
  public UserController(IUserUseCase userUseCase)
  {
    _userUseCase = userUseCase;
  }

  [HttpPost("/Auth")]
  [AllowAnonymous]
  public async Task<ActionResult<string>> Authenticate([FromBody] AuthenticateViewModel user)
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
  public async Task<ActionResult<User>> Create([FromBody] CreateUserViewModel user)
  {
    try
    {
      var created = await _userUseCase.Create(user);

      return Created($"/User/{created.UserId}", new ListUserViewModel(){
        Id = created.UserId,
        Username = created.Username,
        Email = created.Email,
        Name = created.Name,
        Role = created.Role,
        Module = created.Module,
        Status = created.Status
      });
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpGet]
  [Authorize]
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
  [Authorize]
  public async Task<ActionResult<ListUserWithPostsViewModel>> GetById([FromRoute] string id)
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
  [Authorize]
  public async Task<ActionResult<List<ListUserViewModel>>> GetByName([FromRoute] string name)
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
  [Authorize]
  public async Task<ActionResult<ListUserViewModel>> Update([FromRoute] string id, [FromBody] UpdateUserViewModel userToUpdate)
  {
    try
    {
      var authorizationService = new AuthorizationService();
      var isValidUser = authorizationService.VerifyIdentity(id, User);
      if (!isValidUser)
      {
        return Unauthorized();
      }


      var user = await _userUseCase.Update(id, userToUpdate);

      if (user is null)
      {
        return NotFound("User not found");
      }

      return Ok(new ListUserViewModel() {
        Id = user.UserId,
        Username = user.Username,
        Email = user.Email,
        Name = user.Name,
        Role = user.Role,
        Module = user.Module,
        Status = user.Status
      });
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpPut("{id}/Role")]
  [Authorize]
  public async Task<ActionResult<ListUserViewModel>> UpdateRole([FromRoute] string id, [FromBody] string role)
  {
      if (!User.IsInRole("Admin"))
      {
        return Unauthorized();
      }

      var userUpdated = await _userUseCase.UpdateRole(id, role);

      if (userUpdated is null)
      {
        return NotFound("User not found");
      }

      return Ok(new ListUserViewModel() {
        Id = userUpdated.UserId,
        Username = userUpdated.Username,
        Email = userUpdated.Email,
        Name = userUpdated.Name,
        Role = userUpdated.Role,
        Module = userUpdated.Module,
        Status = userUpdated.Status
      });

  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<ActionResult> Delete([FromRoute] string id)

  {
    try
    {
      var authorizationService = new AuthorizationService();
      var isValidUser = authorizationService.VerifyIdentity(id, User);
      if (!isValidUser)
      {
        return Unauthorized();
      }
      
      var user = await _userUseCase.Delete(id);

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