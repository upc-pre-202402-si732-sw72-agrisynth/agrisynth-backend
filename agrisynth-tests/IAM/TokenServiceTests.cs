using agrisynth_backend.IAM.Infrastructure.Hashing.BCrypt.Services;

namespace agrisynth_tests.IAM;

using Xunit;
using Moq;
using Microsoft.Extensions.Options;
using agrisynth_backend.IAM.Infrastructure.Tokens.JWT.Configuration;
using agrisynth_backend.IAM.Infrastructure.Tokens.JWT.Services;
using agrisynth_backend.IAM.Domain.Model.Aggregates;

public class TokenServiceTests
{
    private readonly Mock<IOptions<TokenSettings>> _mockTokenSettings;
    private readonly TokenService _tokenService;
    private readonly HashingService _hashingService;

    public TokenServiceTests()
    {
        _mockTokenSettings = new Mock<IOptions<TokenSettings>>();
        _mockTokenSettings.Setup(x => x.Value).Returns(new TokenSettings { Secret = "supersecretkey" });
        _tokenService = new TokenService(_mockTokenSettings.Object);
        _hashingService = new HashingService();
    }

    [Fact]
    public void GenerateToken_EmptySecret_ThrowsException()
    {
        _mockTokenSettings.Setup(x => x.Value).Returns(new TokenSettings { Secret = "" });
        var tokenService = new TokenService(_mockTokenSettings.Object);
        var passwordHashed = _hashingService.HashPassword("password");
        var user = new User("test1", passwordHashed);
        Assert.Throws<Exception>(() => tokenService.GenerateToken(user));
    }

    [Fact]
    public async Task ValidateToken_InvalidToken_ReturnsNull()
    {
        var userId = await _tokenService.ValidateToken("invalidtoken");
        Assert.Null(userId);
    }

    [Fact]
    public async Task ValidateToken_EmptyToken_ReturnsNull()
    {
        var userId = await _tokenService.ValidateToken("");
        Assert.Null(userId);
    }
    
}