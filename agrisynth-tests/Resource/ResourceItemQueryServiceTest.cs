using agrisynth_backend.Resource.Application.QueryServices;
using agrisynth_backend.Resource.Domain.Model.Aggregates;
using agrisynth_backend.Resource.Domain.Model.Commands;
using agrisynth_backend.Resource.Domain.Model.Queries;
using agrisynth_backend.Resource.Domain.Repositories;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace agrisynth_tests.Resource;

public class ResourceItemQueryServiceTest
{
    private readonly ResourceItemQueryService _service;
    private readonly Mock<IResourceItemRepository> _resourceItemRepositoryMock;

    public ResourceItemQueryServiceTest()
    {
        _resourceItemRepositoryMock = new Mock<IResourceItemRepository>();
        _service = new ResourceItemQueryService(_resourceItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_GetAllResourceItemsQuery_ShouldReturnAllResourceItems()
    {
        // Arrange
        var expectedItems = new List<ResourceItem>
        {
            new ResourceItem(new CreateResourceItemCommand("Item 1", 10, "Type1", "10.00", "15.00", "imageUrl1")),
            new ResourceItem(new CreateResourceItemCommand("Item 2", 5, "Type2", "20.00", "25.00", "imageUrl2"))
        };

        _resourceItemRepositoryMock
            .Setup(repo => repo.ListAsync())
            .ReturnsAsync(expectedItems);

        var query = new GetAllResourceItemsQuery();

        // Act
        var result = await _service.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(expectedItems, result); // Verifica que los items retornados coincidan
    }

    [Fact]
    public async Task Handle_GetResourceItemByIdQuery_ShouldReturnResourceItem_WhenIdExists()
    {
        // Arrange
        var expectedItem = new ResourceItem(new CreateResourceItemCommand("Item 1", 10, "Type1", "10.00", "15.00", "imageUrl1"));

        _resourceItemRepositoryMock
            .Setup(repo => repo.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(expectedItem);

        var query = new GetResourceItemByIdQuery(1);

        // Act
        var result = await _service.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedItem, result); // Verifica que el item retornado sea el esperado
    }

    [Fact]
    public async Task Handle_GetResourceItemByIdQuery_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        _resourceItemRepositoryMock
            .Setup(repo => repo.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ResourceItem?)null); // Simula que no se encuentra el item

        var query = new GetResourceItemByIdQuery(1);

        // Act
        var result = await _service.Handle(query);

        // Assert
        Assert.Null(result); // Verifica que el resultado es null cuando no se encuentra el item
    }
}