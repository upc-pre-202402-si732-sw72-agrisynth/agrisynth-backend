namespace agrisynth_tests.IAM;

using agrisynth_backend.IAM.Infrastructure.Hashing.BCrypt.Services;
using Xunit;
using Assert = Xunit.Assert;

public class HashingServiceTests
{
    private readonly HashingService _hashingService = new();

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        var password = "password123";
        var hashedPassword = _hashingService.HashPassword(password);

        Assert.False(string.IsNullOrEmpty(hashedPassword));
        Assert.NotEqual(password, hashedPassword);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_WhenPasswordMatchesHash()
    {
        var password = "password123";
        var hashedPassword = _hashingService.HashPassword(password);

        var result = _hashingService.VerifyPassword(password, hashedPassword);

        Assert.True(result);
    }
}