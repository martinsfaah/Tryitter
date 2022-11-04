using Tryitter.Models;
using Microsoft.EntityFrameworkCore;

namespace Tryitter.Repositories;

public class PostRepository : IPostRepository
{
  private TryitterContext _context;
  public PostRepository(TryitterContext context)
  {
    _context = context;
  }

  public async Task<Post> Create(Post post)
  {
    await _context.Posts.AddAsync(post);
    await _context.SaveChangesAsync();

    return post;
  }
  
  public async Task<List<Post>> GetAll()
  {
    var posts = await _context.Posts.ToListAsync();

    return posts;
  }

  public async Task<Post?> GetById(int id)
  {
    var post = await _context.Posts.Where(x => x.PostId == id).FirstOrDefaultAsync();

    return post;
  }

  public async Task<Post> Update(Post post)
  {
    _context.Update(post);
    await _context.SaveChangesAsync();

    return post;
  }

  public async Task<Post> Delete(Post post)
  {
    _context.Remove(post);
    await _context.SaveChangesAsync();
    return post;
  }
}