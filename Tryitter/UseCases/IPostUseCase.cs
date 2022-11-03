using Tryitter.Models;
using Tryitter.RequestHandlers;

namespace Tryitter.UseCases;

public interface IPostUseCase
{
  Task<Post?> Create(Postrequest post);
  List<Post> GetAll();
  Post? GetById(int id);
  Post? Update(int id, Post newPost);
  Post? Delete(int id);
}