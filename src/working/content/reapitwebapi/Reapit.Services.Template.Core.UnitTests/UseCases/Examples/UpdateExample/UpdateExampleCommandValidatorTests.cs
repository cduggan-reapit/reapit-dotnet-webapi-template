using Reapit.Services.Template.Core.Helpers;
using Reapit.Services.Template.Core.UnitTests.TestHelpers;
using Reapit.Services.Template.Core.UseCases;
using Reapit.Services.Template.Core.UseCases.Examples.CreateExample;
using Reapit.Services.Template.Core.UseCases.Examples.UpdateExample;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UnitTests.UseCases.Examples.UpdateExample;

public class UpdateExampleCommandValidatorTests
{
    [Fact]
    public async Task Validation_ShouldPass_WhenCommandValid()
    {
        var example = Example.SeedData.Skip(2).First();
        var command = new UpdateExampleCommand(example.Id, "new name", DateTime.UnixEpoch, example.GetEtag());
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);
        actual.IsValid.Should().BeTrue();
    }
    
    // Name
    
    [Fact]
    public async Task Validation_ShouldFail_WhenNameExceedsMaximumLength()
    {
        var example = Example.SeedData.Skip(2).First();
        var command = new UpdateExampleCommand(example.Id, new string('-', Constants.NameMaximumLength + 1), DateTime.UnixEpoch, example.GetEtag());
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);
        actual.ShouldHaveOneErrorWithMessage(nameof(CreateExampleCommand.Name), ValidationMessages.ValueExceedsMaximumLengthOf(Constants.NameMaximumLength));
    }

    [Fact] public async Task Validation_ShouldFail_WhenNameAlreadyInUse()
    {
        var name = Example.SeedData.First().Name;
        var example = Example.SeedData.Skip(2).First();
        var command = new UpdateExampleCommand(example.Id, name, DateTime.UnixEpoch, example.GetEtag());
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);
        actual.ShouldHaveOneErrorWithMessage(nameof(CreateExampleCommand.Name), ValidationMessages.ValueMustBeUnique);
    }
    
    // Private Methods

    private UpdateExampleCommandValidator CreateSut()
        => new();
}