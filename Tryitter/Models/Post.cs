namespace Tryitter.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; } // Imagem já virá convertida
        public string? ContentType { get; set; }
        public User User { get; set; }
    }
}