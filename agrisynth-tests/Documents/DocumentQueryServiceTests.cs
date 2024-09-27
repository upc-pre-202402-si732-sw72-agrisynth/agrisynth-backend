using agrisynth_backend.Documents.Application.QueryServices;
using agrisynth_backend.Documents.Domain.Model.Aggregates;
using agrisynth_backend.Documents.Domain.Model.Commands;
using agrisynth_backend.Documents.Domain.Model.Queries;
using agrisynth_backend.Documents.Domain.Repositories;

namespace agrisynth_tests.Documents;

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

public class DocumentQueryServiceTests
{
    private readonly Mock<IDocumentRepository> _documentRepositoryMock;
    private readonly DocumentQueryService _documentQueryService;

    public DocumentQueryServiceTests()
    {
        _documentRepositoryMock = new Mock<IDocumentRepository>();
        _documentQueryService = new DocumentQueryService(_documentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_GetAllDocumentsQuery_ShouldReturnAllDocuments()
    {
        // Arrange
        var documents = new List<Document>
        {
            new Document(new CreateDocumentCommand ( "Document 1" )),
            new Document(new CreateDocumentCommand ("Document 2" ))
        };
        _documentRepositoryMock.Setup(repo => repo.ListAsync()).ReturnsAsync(documents);

        // Act
        var result = await _documentQueryService.Handle(new GetAllDocumentsQuery());

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_GetDocumentByIdQuery_ShouldReturnDocument()
    {
        // Arrange
        var document = new Document(new CreateDocumentCommand ( "Test Document" ));
        _documentRepositoryMock.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(document);

        // Act
        var result = await _documentQueryService.Handle(new GetDocumentByIdQuery (  1 ));

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Document", result.Name);
    }
}