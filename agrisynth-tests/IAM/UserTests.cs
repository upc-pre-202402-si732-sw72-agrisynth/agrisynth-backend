using agrisynth_backend.IAM.Domain.Model.Aggregates;
using Xunit;
using Assert = Xunit.Assert;

namespace agrisynth_tests.IAM;

public class UserTests
{
    [Fact]
    public void Constructor_ValidParameters_PropertiesAreSet()
    {
        var user = new User("testuser", "hashedpassword");
        Assert.Equal("testuser", user.Username);
        Assert.Equal("hashedpassword", user.PasswordHash);
    }

    [Fact]
    public void Constructor_DefaultConstructor_PropertiesAreEmpty()
    {
        var user = new User();
        Assert.Equal(string.Empty, user.Username);
        Assert.Equal(string.Empty, user.PasswordHash);
    }

    [Fact]
    public void UpdatePasswordHash_ValidPasswordHash_UpdatesPasswordHash()
    {
        var user = new User("testuser", "oldhash");
        user.UpdatePasswordHash("newhash");
        Assert.Equal("newhash", user.PasswordHash);
    }

    [Fact]
    public void UpdateUsername_ValidUsername_UpdatesUsername()
    {
        var user = new User("olduser", "hashedpassword");
        user.UpdateUsername("newuser");
        Assert.Equal("newuser", user.Username);
    }
}