using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Reapit.Packages.ErrorHandling.Exceptions;
using Reapit.Services.Template.Core.Helpers;
using Reapit.Services.Template.Core.UseCases.Examples.PatchExample;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UnitTests.UseCases.Examples.PatchExample;

public class PatchExampleCommandHandlerTests
{
    // Handle

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenExampleNotExists()
    {
        var command = new PatchExampleCommand("ABC123", "", new JsonPatchDocument<Example>());
        var sut = CreateSut();
        var action = () => sut.Handle(command, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task Handle_ThrowsConflictException_WhenEtagIncorrect()
    {
        var entity = Example.SeedData.Skip(2).First();
        var command = new PatchExampleCommand(entity.Id, "", new JsonPatchDocument<Example>());
        var sut = CreateSut();
        var action = () => sut.Handle(command, default);
        await action.Should().ThrowAsync<ConflictException>();
    }
    
    [Fact]
    public async Task Handle_ReturnsUpdatedEntity()
    {
        var entity = Example.SeedData.Skip(2).First();
        var command = new PatchExampleCommand(entity.Id, entity.GetEtag(), new JsonPatchDocument<Example>
        {
            Operations =
            {
                new Operation<Example>("replace", "/name", "", "newName!"),
                new Operation<Example>("replace", "/date", "", "2020-01-01")
            }
        });
        
        var sut = CreateSut();
        var actual = await sut.Handle(command, default);
        actual.Name.Should().BeEquivalentTo("newName!");
        actual.Date.Should().Be(new DateTime(2020, 1, 1));
    }
    
    [Fact]
    public async Task Handle_ThrowsJsonPatchException_WhenPatchDataInvalid()
    {
        var entity = Example.SeedData.Skip(2).First();
        var command = new PatchExampleCommand(entity.Id, entity.GetEtag(), new JsonPatchDocument<Example>
        {
            Operations =
            {
                new Operation<Example>("replace", "/name", "", "newName!"),
                new Operation<Example>("replace", "/date", "", "not a date")
            }
        });
        
        var sut = CreateSut();
        var action = () => sut.Handle(command, default);
        await action.Should().ThrowAsync<JsonPatchException>();
    }
    
    // Private Methods

    private static PatchExampleCommandHandler CreateSut() => new();
}