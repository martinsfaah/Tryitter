namespace Tryitter.ViewModels.User;

using System.ComponentModel.DataAnnotations;

public class CreateUserViewModel
{
    [Required] public string Username { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Password { get; set; }
    public string Module { get; set; }
    public string Status { get; set; }
}