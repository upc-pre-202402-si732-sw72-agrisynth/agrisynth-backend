using agrisynth_backend.IAM.Application.Internal.OutboundServices;

namespace agrisynth_tests.IAM;

using System;
using System.Threading.Tasks;
using agrisynth_backend.IAM.Application.Internal.CommandServices;
using agrisynth_backend.IAM.Domain.Model.Commands;
using agrisynth_backend.IAM.Domain.Model.Aggregates;
using agrisynth_backend.IAM.Domain.Repositories;
using agrisynth_backend.Shared.Domain.Repositories;
using Moq;
using Xunit;

public class UserCommandServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IHashingService> _hashingServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UserCommandService _userCommandService;

    public UserCommandServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _hashingServiceMock = new Mock<IHashingService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userCommandService = new UserCommandService(
            _userRepositoryMock.Object,
            _hashingServiceMock.Object,
            _tokenServiceMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_SignUpCommand_ShouldCreateUser()
    {
        // Arrange
        var command = new SignUpCommand("testuser", "password");
        _userRepositoryMock.Setup(repo => repo.ExistsByUsername(command.Username)).Returns(false);
        _hashingServiceMock.Setup(service => service.HashPassword(command.Password)).Returns("hashedpassword");

        // Act
        await _userCommandService.Handle(command);

        // Assert
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_SignInCommand_ShouldReturnAuthenticatedUser()
    {
        // Arrange
        var command = new SignInCommand("testuser", "password");
        var user = new User("testuser", "hashedpassword");
        _userRepositoryMock.Setup(repo => repo.FindByUsernameAsync(command.Username)).ReturnsAsync(user);
        _hashingServiceMock.Setup(service => service.VerifyPassword(command.Password, user.PasswordHash)).Returns(true);
        _tokenServiceMock.Setup(service => service.GenerateToken(user)).Returns("token");

        // Act
        var result = await _userCommandService.Handle(command);

        // Assert
        Assert.Equal(user, result.user);
        Assert.Equal("token", result.token);
    }
}