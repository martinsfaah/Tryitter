using Tryitter.Models;
using Tryitter.RequestHandlers;

namespace Tryitter.UseCases;

public interface IPostUseCase
{
  Task<Post?> Create(Postrequest post);
  Task<List<Post>> GetAll();
  Task<Post?> GetById(int id);
  Task<Post?> Update(int id, Post newPost);
  Task<Post?> Delete(int id);
}