using Moq;
using Microsoft.AspNetCore.Mvc;
using agrisynth_backend.Profiles.Domain.Model.Aggregates;
using agrisynth_backend.Profiles.Domain.Model.Commands;
using agrisynth_backend.Profiles.Domain.Model.Queries;
using agrisynth_backend.Profiles.Domain.Services;
using agrisynth_backend.Profiles.Interfaces.REST;
using agrisynth_backend.Profiles.Interfaces.REST.Resources;
using Xunit;
using Assert = Xunit.Assert;

namespace agrisynth_tests.Profiles
{
    public class ProfilesControllerTests
    {
        private readonly Mock<IProfileCommandService> _mockProfileCommandService;
        private readonly Mock<IProfileQueryService> _mockProfileQueryService;
        private readonly ProfilesController _controller;

        public ProfilesControllerTests()
        {
            _mockProfileCommandService = new Mock<IProfileCommandService>();
            _mockProfileQueryService = new Mock<IProfileQueryService>();
            _controller = new ProfilesController(_mockProfileCommandService.Object, _mockProfileQueryService.Object);
        }

        [Fact]
        public async Task CreateProfile_ReturnsCreatedAtAction_WhenProfileIsCreated()
        {
            // Arrange
            var resource = new CreateProfileResource("John", "Doe", "johndoe", "john@example.com", 1, "123456789", "ID123");
            var command = new CreateProfileCommand(resource.FirstName, resource.LastName, resource.UserName, resource.Email, resource.AreaCode, resource.Number, resource.IdentityNumber);
            var profile = new Profile(command);

            _mockProfileCommandService.Setup(service => service.Handle(It.IsAny<CreateProfileCommand>())).ReturnsAsync(profile);

            // Act
            var result = await _controller.CreateProfile(resource) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("GetProfileById", result.ActionName);
        }

        [Fact]
        public async Task CreateProfile_ReturnsBadRequest_WhenProfileIsNull()
        {
            // Arrange
            var resource = new CreateProfileResource("John", "Doe", "johndoe", "john@example.com", 1, "123456789", "ID123");

            _mockProfileCommandService.Setup(service => service.Handle(It.IsAny<CreateProfileCommand>())).ReturnsAsync((Profile)null);

            // Act
            var result = await _controller.CreateProfile(resource) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
    }
}
