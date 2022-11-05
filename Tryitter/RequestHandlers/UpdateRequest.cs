namespace Tryitter.RequestHandlers;

public class UpdateRequest
{
  public string? Email { get; set; }
  public string? Name { get; set; }
  public string? Module { get; set; }
  public string? Status { get; set; }
}