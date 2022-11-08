namespace Tryitter.ViewModels.Post;

using Tryitter.ViewModels.User;

public class ListPostViewModel
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public string ContentType { get; set; }
    public ShowUserViewModel User {get; set;}
}