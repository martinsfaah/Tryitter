using Tryitter.Repositories;
using Tryitter.Repositories.Mock;

namespace Tryitter.UseCases.Test;

public class TestUserUseCase {
  private readonly IUserRepository _userRepository;

  public TestUserUseCase() {
    _userRepository = new UserRepositoryMock();
  }

  [Fact]
  public async Task ShouldAuthenticateUser() {
    var sut = new UserUseCase(_userRepository);

    var result = await sut.Auth("johndoe@example.com", "123456");

    result!.Split(".").Length.Should().Be(3);
  }
}