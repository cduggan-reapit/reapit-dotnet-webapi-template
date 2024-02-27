using Reapit.Packages.ErrorHandling.Exceptions;
using Reapit.Services.Template.Core.Helpers;
using Reapit.Services.Template.Core.UseCases.Examples.DeleteExample;
using Reapit.Services.Template.Domain.Entities.Examples;
using Reapit.Services.Template.Domain.Providers;

namespace Reapit.Services.Template.Core.UnitTests.UseCases.Examples.DeleteExample;

public class DeleteExampleCommandHandlerTests
{
    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenExampleNotFound()
    {
        const string id = "test_id";
        const string etag = "test_etag";

        var sut = CreateSut();
        var action = () => sut.Handle(new DeleteExampleCommand(id, etag), default);
        await action.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task Handle_ThrowsConflictException_WhenEtagInvalid()
    {
        var id = Example.SeedData.First().Id;
        const string etag = "test_etag";

        var sut = CreateSut();
        var action = () => sut.Handle(new DeleteExampleCommand(id, etag), default);
        await action.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task Handle_ReturnsCompletedTask_WhenDeleteSuccessful()
    {
        using var timeContext = new SystemTimeProviderContext(DateTimeOffset.UnixEpoch);
        var example = Example.SeedData.First();
        
        var sut = CreateSut();
        await sut.Handle(new DeleteExampleCommand(example.Id, example.GetEtag()), default);
    }
    
    // Private Methods

    private DeleteExampleCommandHandler CreateSut()
        => new();
}