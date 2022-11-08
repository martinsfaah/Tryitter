using Tryitter.Models;
using Tryitter.ViewModels.Post;

namespace Tryitter.UseCases;

public interface IPostUseCase
{
  Task<Post> Create(string Content, string ImageUrl, string userId);
  Task<List<ListPostViewModel>> GetAll();
  Task<ListPostViewModel?> GetById(string id);
  Task<Post?> Update(string id, string content);
  Task Delete(string id);
}