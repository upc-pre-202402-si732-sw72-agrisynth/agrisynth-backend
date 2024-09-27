using agrisynth_backend.Profiles.Application.Internal.QueryServices;
using agrisynth_backend.Profiles.Domain.Model.Aggregates;
using agrisynth_backend.Profiles.Domain.Model.Queries;
using agrisynth_backend.Profiles.Domain.Repositories;
using agrisynth_backend.Profiles.Domain.Model.ValueObjects;
using Moq;
using Xunit;
using Assert = Xunit.Assert;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agrisynth_tests.Profiles
{
    public class ProfileQueryServiceTests
    {
        private readonly Mock<IProfileRepository> _mockProfileRepository;
        private readonly ProfileQueryService _profileQueryService;

        public ProfileQueryServiceTests()
        {
            _mockProfileRepository = new Mock<IProfileRepository>();
            _profileQueryService = new ProfileQueryService(_mockProfileRepository.Object);
        }

        [Fact]
        public async Task Handle_GetAllProfilesQuery_ReturnsListOfProfiles()
        {
            // Arrange
            var profiles = new List<Profile> { new Profile("John", "Doe", "johndoe", "john@example.com", 1, "123456789", "ID123") };
            _mockProfileRepository.Setup(repo => repo.ListAsync()).ReturnsAsync(profiles);

            var query = new GetAllProfilesQuery();

            // Act
            var result = await _profileQueryService.Handle(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task Handle_GetProfileByEmailQuery_ReturnsProfile()
        {
            // Arrange
            var email = new EmailAddress("john@example.com");
            var profile = new Profile("John", "Doe", "johndoe", email.ToString(), 1, "123456789", "ID123");

            _mockProfileRepository.Setup(repo => repo.FindProfileByEmailAsync(It.IsAny<EmailAddress>())).ReturnsAsync(profile);

            var query = new GetProfileByEmailQuery(email);

            // Act
            var result = await _profileQueryService.Handle(query);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(email.Address, result.Email.Address);
        }

        [Fact]
        public async Task Handle_GetProfileByIdQuery_ReturnsProfile()
        {
            // Arrange
            var profile = new Profile("John", "Doe", "johndoe", "john@example.com", 1, "123456789", "ID123");
            _mockProfileRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(profile);

            var query = new GetProfileByIdQuery(1);

            // Act
            var result = await _profileQueryService.Handle(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("johndoe", result.Name.UserName);
        }
    }
}
