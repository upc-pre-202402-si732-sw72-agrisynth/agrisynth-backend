using agrisynth_backend.Documents.Application.CommandServices;
using agrisynth_backend.Documents.Domain.Model.Aggregates;
using agrisynth_backend.Documents.Domain.Model.Commands;
using agrisynth_backend.Documents.Domain.Repositories;
using agrisynth_backend.Shared.Domain.Repositories;

namespace agrisynth_tests.Documents;

using System;
using System.Threading.Tasks;
using Moq;
using Xunit;

public class DocumentCommandServiceTests
{
    private readonly Mock<IDocumentRepository> _documentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DocumentCommandService _documentCommandService;

    public DocumentCommandServiceTests()
    {
        _documentRepositoryMock = new Mock<IDocumentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _documentCommandService = new DocumentCommandService(_documentRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_CreateDocumentCommand_ShouldCreateDocument()
    {
        // Arrange
        var command = new CreateDocumentCommand ( "Test Document" );
        var document = new Document(command);

        // Act
        var result = await _documentCommandService.Handle(command);

        // Assert
        _documentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Document>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        Assert.NotNull(result);
        Assert.Equal("Test Document", result.Name);
    }

    [Fact]
    public async Task Handle_UpdateDocumentCommand_ShouldUpdateDocument()
    {
        // Arrange
        var command = new UpdateDocumentCommand ( 1,"Updated Document" );
        var document = new Document(command);
        _documentRepositoryMock.Setup(repo => repo.FindByIdAsync(command.Id)).ReturnsAsync(document);

        // Act
        var result = await _documentCommandService.Handle(command);

        // Assert
        _documentRepositoryMock.Verify(repo => repo.Update(It.IsAny<Document>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        Assert.NotNull(result);
        Assert.Equal("Updated Document", result.Name);
    }

    [Fact]
    public async Task Handle_DeleteDocumentCommand_ShouldDeleteDocument()
    {
        // Arrange
        var command = new DeleteDocumentCommand ( 1 );
        var document = new Document(new CreateDocumentCommand ("Test Document" ));
        _documentRepositoryMock.Setup(repo => repo.FindByIdAsync(command.Id)).ReturnsAsync(document);

        // Act
        var result = await _documentCommandService.Handle(command);

        // Assert
        _documentRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Document>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        Assert.NotNull(result);
    }
}