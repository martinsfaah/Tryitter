namespace Tryitter.RequestHandlers;

public class PostCreateRequest
{
  public string? Content { get; set; }
  public IFormFile ImageUrl { get; set; }
  public int UserId { get; set; }
}