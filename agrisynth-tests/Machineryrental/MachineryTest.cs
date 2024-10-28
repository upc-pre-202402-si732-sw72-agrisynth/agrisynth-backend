using agrisynth_backend.Machineryrental.Domain.Model.Aggregates;
using agrisynth_backend.Machineryrental.Domain.Model.Commands;
using Xunit;
using Assert = Xunit.Assert;

namespace agrisynth_tests.Machineryrental;

public class MachineryTest
{
    [Fact]
    public void Constructor_ValidCommand_PropertiesAreSet()
    {
        var command = new CreateMachineryCommand(  "Tractor", 1000, "https://example.com/tractor.jpg");
        var machinery = new Machinery(command);
        Assert.Equal("Tractor", machinery.Name);
        Assert.Equal(1000, machinery.Price);
        Assert.Equal("https://example.com/tractor.jpg", machinery.ImageUrl);
    }

    [Fact]
    public void Constructor_DefaultConstructor_PropertiesAreEmpty()
    {
        var machinery = new Machinery();
        Assert.Equal(string.Empty, machinery.Name);
        Assert.Equal(0, machinery.Price);
        Assert.Equal(string.Empty, machinery.ImageUrl);
    }
}