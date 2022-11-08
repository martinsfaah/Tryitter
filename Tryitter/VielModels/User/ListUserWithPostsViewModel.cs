namespace Tryitter.ViewModels.User;

using Tryitter.ViewModels.Post;

public class ListUserWithPostsViewModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Module { get; set; }
    public string Status { get; set; }
    public IEnumerable<ShowPostViewModel> Posts { get; set; }
}