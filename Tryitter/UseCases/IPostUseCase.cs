using Tryitter.Models;

namespace Tryitter.UseCases;

public interface IPostUseCase
{
  Post? Create(Post post);
  List<Post> GetAll();
  Post? GetById(int id);
  Post? Update(int id, Post newPost);
}