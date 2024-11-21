using agrisynth_backend.Collaboration.Application.CommandServices;
using agrisynth_backend.Collaboration.Domain.Model.Commands;
using agrisynth_backend.Collaboration.Domain.Model.Entities;
using agrisynth_backend.Collaboration.Domain.Repositories;
using agrisynth_backend.Shared.Domain.Repositories;
using Moq;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace agrisynth_tests.Collaboration;

public class TeamCommandServiceTest
{
    private readonly Mock<ITeamRepository> _teamRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly TeamCommandService _teamCommandService;
    
    public TeamCommandServiceTest()
    {
        _teamRepositoryMock = new Mock<ITeamRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _teamCommandService = new TeamCommandService(_teamRepositoryMock.Object, _unitOfWorkMock.Object);
    }
    
    [Fact] 
    // este test es para probar que se crea un equipo solo el nombre 
    public async Task CreateTeamCommand_SuccessfullyCreatesTeam()
    {
        // Arrange
        var command = new CreateTeamCommand(
            "Team 1");
        var team = new Team(command);

        _teamRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Team>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _teamCommandService.Handle(command);

       // Assert
       Xunit.Assert.NotNull(result);
       Xunit.Assert.Equal("Team 1", result.Name);
        _teamRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Team>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
    }
}