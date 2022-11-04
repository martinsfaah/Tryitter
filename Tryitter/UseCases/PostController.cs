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
  public async Task<ActionResult<Post>> Create([FromForm] PostCreateRequest post)
  {
    try
    {
      
      var created = await _postUseCase.Create(post);

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
  public async Task<ActionResult<List<Post>>> GetAll()
  {
    try
    {
      var posts = await _postUseCase.GetAll();

      return Ok(posts);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<Post>> GetById([FromRoute] string id)
  {
    try
    {
      int IdNumber = Convert.ToInt32(id);
      
      var post = await _postUseCase.GetById(IdNumber);

      if (post is null)
      {
        return NotFound("Post not found");
      }

      return Ok(post);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpGet("Image/{id}")]
  [AllowAnonymous]
  public async Task<ActionResult> GetImage([FromRoute] string id)
  {
    try
    {
      int IdNumber = Convert.ToInt32(id);
      
      var post = await _postUseCase.GetById(IdNumber);

      if (post is null)
      {
        return NotFound("Post not found");
      }

      return File(post.ImageUrl, post.ContentType);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpPut("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<Post>> Update([FromRoute] string id,[FromBody] PostUpdateRequest newPost)
  {
    try
    {
      int IdNumber = Convert.ToInt32(id);
      
      var post = await _postUseCase.Update(IdNumber, newPost);

      if (post is null)
      {
        return NotFound("Post not found");
      }

      return Ok(post);
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
      
      var post = await _postUseCase.Delete(IdNumber);

      if (post is null)
      {
        return NotFound("Post not found");
      }

      return NoContent();
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }
}