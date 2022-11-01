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
  

}