using Tryitter.Models;
using Tryitter.ViewModels.Post;

namespace Tryitter.Repositories;

public interface IPostRepository
{
  Task<Post> Create(string Content, string ImageUrl, int userId);
  Task<List<ListPostViewModel>> GetAll();
  Task<ListPostViewModel?> GetById(int id);
  Task<Post> Update(int postId, string content);
  Task<Post> Delete(int postId);
}