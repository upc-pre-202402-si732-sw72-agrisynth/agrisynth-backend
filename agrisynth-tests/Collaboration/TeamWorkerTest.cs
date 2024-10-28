using agrisynth_backend.Collaboration.Domain.Model.Aggregates;
using agrisynth_backend.Collaboration.Domain.Model.Commands;
using Xunit;
using Assert = Xunit.Assert;

namespace agrisynth_tests.Collaboration;

public class TeamWorkerTest
{
    [Fact]
    public void Constructor_ValidParameters_PropertiesAreSet()
    {
        var teamWorker = new TeamWorker(1, 2);
        Assert.Equal(1, teamWorker.TeamId);
        Assert.Equal(2, teamWorker.WorkerId);
    }

    [Fact]
    public void Constructor_CommandParameter_PropertiesAreSet()
    {
        var command = new CreateTeamWorkerCommand(   1, 2 );
        var teamWorker = new TeamWorker(command);
        Assert.Equal(1, teamWorker.TeamId);
        Assert.Equal(2, teamWorker.WorkerId);
    }

    [Fact]
    public void Update_ValidCommand_PropertiesAreUpdated()
    {
        var teamWorker = new TeamWorker(1, 2);
        var command = new UpdateTeamWorkerCommand ( 4, 3, 4);
        teamWorker.Update(command);
        Assert.Equal(3, teamWorker.TeamId);
        Assert.Equal(4, teamWorker.WorkerId);
    }

    [Fact]
    public void Constructor_DefaultConstructor_PropertiesAreZero()
    {
        var teamWorker = new TeamWorker(0, 0);
        Assert.Equal(0, teamWorker.TeamId);
        Assert.Equal(0, teamWorker.WorkerId);
    }
}