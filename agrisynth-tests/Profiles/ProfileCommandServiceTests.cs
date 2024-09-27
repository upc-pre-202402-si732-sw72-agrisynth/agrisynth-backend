using agrisynth_backend.Profiles.Application.Internal.CommandServices;
using agrisynth_backend.Profiles.Domain.Model.Aggregates;
using agrisynth_backend.Profiles.Domain.Model.Commands;
using agrisynth_backend.Profiles.Domain.Repositories;
using agrisynth_backend.Shared.Domain.Repositories;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace agrisynth_tests.Profiles
{
    public class ProfileCommandServiceTests
    {
        private readonly Mock<IProfileRepository> _mockProfileRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ProfileCommandService _profileCommandService;

        public ProfileCommandServiceTests()
        {
            _mockProfileRepository = new Mock<IProfileRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _profileCommandService = new ProfileCommandService(_mockProfileRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_CreateProfileCommand_SuccessfullyCreatesProfile()
        {
            // Arrange
            var command = new CreateProfileCommand("John", "Doe", "johndoe", "john@example.com", 1, "123456789", "ID123");
            var profile = new Profile(command);

            _mockProfileRepository.Setup(repo => repo.AddAsync(It.IsAny<Profile>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _profileCommandService.Handle(command);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("johndoe", result.Name.UserName);
            _mockProfileRepository.Verify(repo => repo.AddAsync(It.IsAny<Profile>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_CreateProfileCommand_ReturnsNull_OnError()
        {
            // Arrange
            var command = new CreateProfileCommand("John", "Doe", "johndoe", "john@example.com", 1, "123456789", "ID123");

            _mockProfileRepository.Setup(repo => repo.AddAsync(It.IsAny<Profile>())).Throws(new Exception("Database error"));

            // Act
            var result = await _profileCommandService.Handle(command);

            // Assert
            Assert.Null(result);
            _mockProfileRepository.Verify(repo => repo.AddAsync(It.IsAny<Profile>()), Times.Once);
        }
    }
}
