namespace Tryitter.RequestHandlers;

public class Postrequest
{
  public string? Content { get; set; }
  public IFormFile ImageUrl { get; set; }
  public int UserId { get; set; }
}