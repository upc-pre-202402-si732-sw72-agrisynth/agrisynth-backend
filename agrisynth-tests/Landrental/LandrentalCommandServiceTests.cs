using agrisynth_backend.Landrental.Application.CommandServices;
using agrisynth_backend.Landrental.Domain.Model.Aggregates;
using agrisynth_backend.Landrental.Domain.Model.Commands;
using agrisynth_backend.Landrental.Domain.Repositories;
using agrisynth_backend.Shared.Domain.Repositories;

namespace agrisynth_tests.Landrental;

using System;
using System.Threading.Tasks;
using Moq;
using Xunit;

public class TerrainCommandServiceTests
{
    private readonly Mock<ITerrainRepository> _terrainRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly TerrainCommandService _terrainCommandService;

    public TerrainCommandServiceTests()
    {
        _terrainRepositoryMock = new Mock<ITerrainRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _terrainCommandService = new TerrainCommandService(_terrainRepositoryMock.Object, _unitOfWorkMock.Object);
    }
    
    
    [Fact]
    public async Task Handle_CreateTerrainCommand_ShouldCreateTerrain()
    {
        // Arrange
        var command = new CreateTerrainCommand (
            "Test Landrental Terrain",
            "Test Landrental Terrain",
            "Lima",
            "Lima",
            1,
            1,
            "Test Landrental Terrain"
            );
        var terrain = new Terrain(command);

        // Act
        var result = await _terrainCommandService.Handle(command);

        // Assert
        _terrainRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Terrain>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        Assert.NotNull(result);
        Assert.Equal("Test Landrental Terrain", result.Name);
    }

    [Fact]
    public async Task Handle_CreateTerrainCommand_ShouldReturnNull_WhenExceptionOccurs()
    {
        // Arrange
        var command = new CreateTerrainCommand(
            "Test Landrental Terrain",
            "Test Landrental Terrain",
            "Lima",
            "Lima",
            1,
            1,
            "Test Landrental Terrain"
            );
        _terrainRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Terrain>())).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _terrainCommandService.Handle(command);

        // Assert
        _terrainRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Terrain>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Never);
        Assert.Null(result);
        }
}