using Reapit.Packages.ErrorHandling.Exceptions;
using Reapit.Services.Template.Core.UseCases.Examples.GetExampleById;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UnitTests.UseCases.Examples.GetExampleById;

public class GetExampleByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenExampleNotFound()
    {
        var query = new GetExampleByIdQuery("test_id");

        var sut = CreateSut();
        var action = () => sut.Handle(query, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ReturnsExample_WhenExampleExists()
    {
        var expected = Example.SeedData.ElementAt(3);
        var query = new GetExampleByIdQuery(expected.Id);

        var sut = CreateSut();
        var actual = await sut.Handle(query, default);
        actual.Should().BeEquivalentTo(expected);
    }
    
    // Private Methods

    private GetExampleByIdQueryHandler CreateSut()
        => new();
}