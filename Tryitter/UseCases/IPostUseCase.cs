using Tryitter.Models;
using Tryitter.RequestHandlers;

namespace Tryitter.UseCases;

public interface IPostUseCase
{
  Task<Post?> Create(PostCreateRequest post);
  Task<List<Post>> GetAll();
  Task<Post?> GetById(int id);
  Task<Post?> Update(int id, PostUpdateRequest newPost);
  Task<Post?> Delete(int id);
}