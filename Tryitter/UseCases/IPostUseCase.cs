using Tryitter.Models;
using Tryitter.RequestHandlers;

namespace Tryitter.UseCases;

public interface IPostUseCase
{
  Task<Post?> Create(PostCreateRequest post, string userId);
  Task<List<Post>> GetAll();
  Task<Post?> GetById(string id);
  Task<Post?> Update(string id, string content);
  Task Delete(Post post);
}