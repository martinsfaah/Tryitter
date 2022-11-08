namespace Tryitter.UseCases;

using Tryitter.Repositories;
using Tryitter.Models;
using Tryitter.ViewModels.Post;

public class PostUseCase : IPostUseCase
{
  private readonly IPostRepository _repository;
  public PostUseCase(IPostRepository repository)
  {
    _repository = repository;
  }
  
  public async Task<Post> Create(string Content, string ImageUrl, string userId)
  {
    var createdPost = await _repository.Create(Content, ImageUrl, Convert.ToInt32(userId));

    if (createdPost is null)
    {
      return null;
    }

    return createdPost;
  }
  
  public async Task<List<ListPostViewModel>> GetAll()
  {
    var posts = await _repository.GetAll();

    return posts;
  }

  public async Task<ListPostViewModel?> GetById(string id)
  {
    int postId = Convert.ToInt32(id);
    var post = await _repository.GetById(postId);

    return post;
  }

  public async Task<Post?> Update(string id, string content)
  {
    int postId = Convert.ToInt32(id);

    var updated = await _repository.Update(postId, content);

    if (updated is null)
    {
      return null;
    }

    return updated;
  }

  public async Task Delete(string id)
  {  
    int postId = Convert.ToInt32(id);
    await _repository.Delete(postId);
  }
}

// ? Virá convertido do frontend
    // Create
    // List<byte[]> data = new(); // TODO Se der tempo verificar sobre cloud storage
    //   if (post.ImageUrl is not null)
    //   {
    //     if (post.ImageUrl.Length > 0)
    //     {
    //       using (var stream = new MemoryStream())
    //       {
    //         await post.ImageUrl.CopyToAsync(stream);

    //         data.Add(stream.ToArray());
    //       }

    //     }
    //   }
    // ? ------------------------------