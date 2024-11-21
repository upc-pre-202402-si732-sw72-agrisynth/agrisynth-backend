using agrisynth_backend.Resource.Application.CommandServices;
using agrisynth_backend.Resource.Domain.Model.Aggregates;
using agrisynth_backend.Resource.Domain.Model.Commands;
using agrisynth_backend.Resource.Domain.Repositories;
using agrisynth_backend.Shared.Domain.Repositories;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace agrisynth_tests.Resource;

public class ResourceItemCommandServiceTest
{
    private readonly Mock<IResourceItemRepository> _resourceItemRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ResourceItemCommandService _service;

    public ResourceItemCommandServiceTest()
    {
        _resourceItemRepositoryMock = new Mock<IResourceItemRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new ResourceItemCommandService(_resourceItemRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_CreateResourceItemCommand_ShouldReturnResourceItem()
    {
        // Arrange
        var command = new CreateResourceItemCommand(
            "Test Item", 
            10,
            "Type1",
            "10.00", 
            "15.00", 
            "imageUrl");
        ResourceItem? resourceItemCaptured = null;

        _resourceItemRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<ResourceItem>()))
            .Callback<ResourceItem>(ri => resourceItemCaptured = ri)
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _service.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Item", result.Name);
        Assert.Equal(10, result.Quantity);
        Assert.Equal("Type1", result.Type);
        Assert.Same(resourceItemCaptured, result);

        // Verificar que se llamaron las interacciones correctas
        _resourceItemRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ResourceItem>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_CreateResourceItemCommand_ShouldReturnNull_WhenExceptionOccurs()
    {
        // Arrange
        var command = new CreateResourceItemCommand("Test Item", 10, "Type1", "10.00", "15.00", "imageUrl");

        _resourceItemRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<ResourceItem>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _service.Handle(command);

        // Assert
        Assert.Null(result);
        
        // Verificar que se intentó agregar el recurso
        _resourceItemRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ResourceItem>()), Times.Once);
    }
}