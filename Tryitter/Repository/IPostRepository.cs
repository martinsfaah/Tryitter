using Tryitter.Models;

namespace Tryitter.Repositories;

public interface IPostRepository
{
  Post Create(Post post);
}