using agrisynth_backend.Landrental.Application.QueryServices;
using agrisynth_backend.Landrental.Domain.Model.Aggregates;
using agrisynth_backend.Landrental.Domain.Model.Commands;
using agrisynth_backend.Landrental.Domain.Model.Queries;
using agrisynth_backend.Landrental.Domain.Repositories;

namespace agrisynth_tests.Landrental;

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using System.Linq;


public class TerrainQueryServiceTests
    {
        private readonly Mock<ITerrainRepository> _terrainRepositoryMock;
        private readonly TerrainQueryService _terrainQueryService;

        public TerrainQueryServiceTests()
        {
            _terrainRepositoryMock = new Mock<ITerrainRepository>();
            _terrainQueryService = new TerrainQueryService(_terrainRepositoryMock.Object);
        }
        /*
        [Fact]
        public async Task Handle_GetAllTerrainsQuery_ShouldReturnAllTerrains()
        {
            // Arrange
            var terrains = new List<Terrain>
            {
                new Terrain("Terrain 1"),
                new Terrain("Terrain 2")
            };
            _terrainRepositoryMock.Setup(repo => repo.ListAsync()).ReturnsAsync(terrains);

            // Act
            var result = await _terrainQueryService.Handle(new GetAllTerrainsQuery());

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Handle_GetTerrainByIdQuery_ShouldReturnTerrain()
        {
            // Arrange
            var terrain = new Terrain("Test Terrain");
            _terrainRepositoryMock.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(terrain);

            // Act
            var result = await _terrainQueryService.Handle(new GetTerrainByIdQuery(1));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Terrain", result.Name);
        }*/
    }