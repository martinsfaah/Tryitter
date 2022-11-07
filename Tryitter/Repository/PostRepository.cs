namespace Tryitter.Repositories;

using Microsoft.EntityFrameworkCore;
using Tryitter.Models;
using Tryitter.Data;
using Tryitter.ViewModels.Post;
using Tryitter.ViewModels.User;

public class PostRepository : IPostRepository
{
  private readonly TryitterContext _context;
  public PostRepository(TryitterContext context)
  {
    _context = context;
  }

  public async Task<Post> Create(string Content, string ImageUrl, int userId)
  {
    var user = await _context.Users.FirstAsync(x => x.UserId == userId);

    var post = new Post()
    {
      Content = Content,
      ImageUrl = ImageUrl,
      User = user
    };

    await _context.Posts.AddAsync(post);
    await _context.SaveChangesAsync();

    return post;
  }
  
  public async Task<List<ListPostViewModel>> GetAll()
  {
    var posts = await _context.Posts
      .AsNoTracking()
      .Select(x => 
        new ListPostViewModel(){
          Id = x.PostId,
          Content = x.Content,
          ImageUrl = x.ImageUrl,
          ContentType = x.ContentType,
          User = new ShowUserViewModel() {
            UserId = x.User.UserId,
            Name = x.User.Name,
            Username = x.User.Username
          },
        }
      )
      .ToListAsync();

    return posts;
  }

  public async Task<ListPostViewModel?> GetById(int id)
  {
    var post = await _context.Posts
    .Select(x => 
        new ListPostViewModel(){
          Id = x.PostId,
          Content = x.Content,
          ImageUrl = x.ImageUrl,
          ContentType = x.ContentType,
          User = new ShowUserViewModel() {
            UserId = x.User.UserId,
            Name = x.User.Name,
            Username = x.User.Username
          },
        }
      )
    .FirstOrDefaultAsync(x => x.Id == id);

    return post;
  }

  public async Task<Post> Update(int postId, string content)
  {
    var postFound = await _context.Posts.FirstAsync(x => x.PostId == postId);

    postFound.Content = content;

    var updated = _context.Update(postFound);
    await _context.SaveChangesAsync();

    return updated.Entity;
  }

  public async Task<Post?> Delete(int postId)
  {
    var postFound = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == postId);
    if (postFound is null)
    {
      return null;
    }

    _context.Remove(postFound);
    await _context.SaveChangesAsync();
    return postFound;
  }
}