using agrisynth_backend.Machineryrental.Application.CommandServices;
using agrisynth_backend.Machineryrental.Domain.Model.Aggregates;
using agrisynth_backend.Machineryrental.Domain.Model.Commands;
using agrisynth_backend.Machineryrental.Domain.Repositories;
using agrisynth_backend.Shared.Domain.Repositories;
using Moq;
using Xunit;

namespace agrisynth_tests.Machineryrental;

public class MachineryCommandServiceTest
{
    private readonly Mock<IMachineryRepository> _machineryRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly MachineryCommandService _machineryCommandService;
    
    public MachineryCommandServiceTest()
    {
        _machineryRepositoryMock = new Mock<IMachineryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _machineryCommandService = new MachineryCommandService(_machineryRepositoryMock.Object, _unitOfWorkMock.Object);
    }
    
    [Fact]
    // este test es para probar que se crea un equipo solo el nombre
    public async Task CreateMachineryCommand_Successfull()
    {
        // Arrange
        var command = new CreateMachineryCommand(
            "Machinery 1", 
            123,
            "www.image.com");
        var machinery = new Machinery(command);

        _machineryRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Machinery>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _machineryCommandService.Handle(command);

       // Assert
       Xunit.Assert.NotNull(result);
       Xunit.Assert.Equal("Machinery 1", result.Name);
        _machineryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Machinery>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
    }
}