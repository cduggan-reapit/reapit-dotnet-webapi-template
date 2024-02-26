using Reapit.Services.Template.Domain.Entities.Examples;
using Reapit.Services.Template.Domain.Providers;

namespace Reapit.Services.Template.Domain.UnitTests.Entities.Examples;

public class ExampleTests
{
    private static readonly DateTimeOffset FixedDate = new(2000, 2, 14, 11, 52, 53, TimeSpan.FromHours(-7));
    
    // Create
    
    [Fact]
    public void Create_PopulatesProperties_FromParameters()
    {
        const string name = "Name";
        var date = new DateTime(1967, 9, 11);
        using var context = new SystemTimeProviderContext(FixedDate);
        
        var example = Example.Create(name, date);
        example.Name.Should().BeEquivalentTo(name);
        example.Date.Should().Be(date);
        example.Created.Should().Be(FixedDate);
        example.Modified.Should().Be(FixedDate);
    }
}