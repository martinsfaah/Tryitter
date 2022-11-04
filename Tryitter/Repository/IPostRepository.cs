using Tryitter.Models;

namespace Tryitter.Repositories;

public interface IPostRepository
{
  Task<Post> Create(Post post);
  Task<List<Post>> GetAll();
  Task<Post?> GetById(int id);
  Task<Post> Update(Post post);
  Task<Post> Delete(Post post);
}