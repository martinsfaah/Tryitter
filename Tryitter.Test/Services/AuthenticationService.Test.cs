using Tryitter.Models;

namespace Tryitter.Services.Test;

public class TestAuthenticationService {

  [Fact]
  public void ShouldTestTokenGenerator() {
    var sut = new AuthenticationService();

    var result = sut.GenerateToken(new User() {
      Username = "johndoe",
      Email = "johndoe@example.com",
      Name = "John Doe"
    });

    result.Split(".").Length.Should().Be(3);
  }
}

