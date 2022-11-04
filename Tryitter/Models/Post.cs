namespace Tryitter.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string? Content { get; set; }
        public byte[] ImageUrl { get; set; }
        public string ContentType { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}