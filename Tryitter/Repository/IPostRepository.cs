using Tryitter.Models;

namespace Tryitter.Repositories;

public interface IPostRepository
{
  Post Create(Post post);
  List<Post> GetAll();
  Post? GetById(int id);
  Post Update(Post post);
  Post Delete(Post post);
}