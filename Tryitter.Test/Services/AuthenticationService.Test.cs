using Tryitter.Models;

namespace Tryitter.Services.Test;

public class TestAuthenticationService {

  [Fact]
  public void ShouldTestTokenGenerator() {
    var sut = new AuthenticationService();

    var result = sut.GenerateToken(new User() {
        UserId = 1,
        Username = "johndoe",
        Email = "johndoe@example.com",
        Role = "User"
    });

         


    result.Split(".").Length.Should().Be(3);
  }
}

