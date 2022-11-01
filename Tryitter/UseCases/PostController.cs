using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


using Tryitter.UseCases;
using Tryitter.Models;
using Tryitter.RequestHandlers;
namespace Tryitter.Controllers;

[Route("[controller]")]
[ApiController]
public class PostController : ControllerBase
{
  private readonly IPostUseCase _postUseCase;
  public PostController(IPostUseCase postUseCase)
  {
    _postUseCase = postUseCase;
  }

  [HttpPost]
  [AllowAnonymous]
  public ActionResult<Post> Create([FromBody] Post post)
  {
    try
    {
      var created = _postUseCase.Create(post);

      if (created is null)
      {
        return NotFound("User not found");
      }

      return CreatedAtAction("GetById", new { id = created.PostId }, created);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpGet]
  [AllowAnonymous]
  public ActionResult<List<Post>> GetAll()
  {
    try
    {
      var posts = _postUseCase.GetAll();

      return Ok(posts);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }


}