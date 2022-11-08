namespace Tryitter.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Tryitter.UseCases;
using Tryitter.Models;
using Tryitter.ViewModels.Post;
using Tryitter.Services;
using Tryitter.ViewModels.User;

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
  public async Task<ActionResult<Post>> Create([FromBody] CreatePostViewModel model)
  {
    try
    {
      var userId = User.FindFirst("Id")!.Value;

      string Content = model.Content;
      string ImageUrl = model.ImageUrl;

      var created = await _postUseCase.Create(Content, ImageUrl, userId);

      if (created is null)
      {
        return NotFound("User not found");
      }

      return Created($"/Post/{created.PostId}", new ListPostViewModel() {
        Id = created.PostId,
        Content = created.Content,
        ImageUrl = created.ImageUrl,
        ContentType = created.ContentType,
        User = new ShowUserViewModel() {
          UserId = created.User.UserId,
          Name = created.User.Name,
          Username = created.User.Username
        }
      });
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
  public async Task<ActionResult<ListPostViewModel>> Update([FromRoute] string id,[FromBody] UpdatePostViewModel model)
  {
    try
    {
      var post = await _postUseCase.GetById(id);

      if (post is null)
      {
        return NotFound("Post not found");
      }

      var authorizationService = new AuthorizationService();
      var isPostUserOrAdmin = authorizationService.VerifyIdentity(post.User.UserId.ToString(), User);
      if(!isPostUserOrAdmin)
      {
        return Unauthorized();
      }

      await _postUseCase.Update(id, model.Content);
      
      var updatedPost = await _postUseCase.GetById(id);

      return Ok(updatedPost);
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
      var isPostUserOrAdmin = authorizationService.VerifyIdentity(post.User.UserId.ToString(), User);
      if(!isPostUserOrAdmin)
      {
        return Unauthorized();
      }
      
      await _postUseCase.Delete(id);

      return NoContent();
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }
}