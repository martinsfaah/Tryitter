using Tryitter.Models;
using Microsoft.EntityFrameworkCore;

namespace Tryitter.Repositories;

public class PostRepository : IPostRepository
{
  private readonly TryitterContext _context;
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
    var posts = await _context.Posts.AsNoTracking().ToListAsync();

    return posts;
  }

  public async Task<Post?> GetById(int id)
  {
    var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == id);

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