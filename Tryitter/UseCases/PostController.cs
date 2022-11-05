namespace Tryitter.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Tryitter.UseCases;
using Tryitter.Models;
using Tryitter.RequestHandlers;
using Tryitter.Services;

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
  [Authorize]
  public async Task<ActionResult<Post>> Create([FromBody] PostCreateRequest post)
  {
    try
    {
      var userId = User.FindFirst("Id")!.Value;

      var created = await _postUseCase.Create(post, userId);

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
  [Authorize]
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
  [Authorize]
  public async Task<ActionResult<Post>> GetById([FromRoute] string id)
  {
    try
    {      
      var post = await _postUseCase.GetById(id);

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

  [HttpPut("{id}")]
  [Authorize]
  public async Task<ActionResult<Post>> Update([FromRoute] string id,[FromBody] UpdatePostRequest updatePostRequest)
  {
    try
    {
      var post = await _postUseCase.GetById(id);

      if (post is null)
      {
        return NotFound("Post not found");
      }

      var authorizationService = new AuthorizationService();
      var isPostUserOrAdmin = authorizationService.VerifyIdentity(post.UserId.ToString(), User);
      if(!isPostUserOrAdmin)
      {
        return Unauthorized();
      }


      var updatedPost = await _postUseCase.Update(id, updatePostRequest.content);

      return Ok(post);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message); 
    }
  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<ActionResult> Delete([FromRoute] string id)
  {
    try
    {
      var post = await _postUseCase.GetById(id);
      if (post is null)
      {
        return NotFound("Post not found");
      }
      
      var authorizationService = new AuthorizationService();
      var isPostUserOrAdmin = authorizationService.VerifyIdentity(post.UserId.ToString(), User);
      if(!isPostUserOrAdmin)
      {
        return Unauthorized();
      }
      
      await _postUseCase.Delete(post);

      return NoContent();
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  // ? Se vir do front não precisa retornar convertido
  // [HttpGet("Image/{id}")]
  // [Authorize]
  // public async Task<ActionResult> GetImage([FromRoute] string id)
  // {
  //   try
  //   {
  //     var post = await _postUseCase.GetById(id);

  //     if (post is null)
  //     {
  //       return NotFound("Post not found");
  //     }

  //     return File(post.ImageUrl, post.ContentType);
  //   }
  //   catch (Exception exception)
  //   {
  //     return BadRequest(exception.Message);
  //   }
  // }
}