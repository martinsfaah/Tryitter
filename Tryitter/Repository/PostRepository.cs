using Tryitter.Models;
using System.Data.Entity;

namespace Tryitter.Repositories;

public class PostRepository : IPostRepository
{
  private TryitterContext _context;
  public PostRepository(TryitterContext context)
  {
    _context = context;
  }

  public Post Create(Post post)
  {
    _context.Posts.Add(post);
    _context.SaveChanges();

    return post;
  }
  
  public List<Post> GetAll()
  {
    var posts = _context.Posts.ToList();

    return posts;
  }

  public Post? GetById(int id)
  {
    var post = _context.Posts.Where(x => x.PostId == id).FirstOrDefault();

    return post;
  }


}