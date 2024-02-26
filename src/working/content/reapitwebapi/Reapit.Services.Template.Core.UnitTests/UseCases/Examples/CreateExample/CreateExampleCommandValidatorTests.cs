using Reapit.Services.Template.Core.UnitTests.TestHelpers;
using Reapit.Services.Template.Core.UseCases;
using Reapit.Services.Template.Core.UseCases.Examples.CreateExample;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UnitTests.UseCases.Examples.CreateExample;

public class CreateExampleCommandValidatorTests
{
    // CreateExampleCommandValidator

    [Fact]
    public async Task CreateExampleCommandValidator_PassesValidation_WhenCommandIsValid()
    {
        var command = new CreateExampleCommand("Name", new DateTime(2013, 1, 7));

        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);

        actual.IsValid.Should().BeTrue();
    }
    
    // Name
    
    [Fact]
    public async Task CreateExampleCommandValidator_FailsValidation_WhenNameEmpty()
    {
        var command = new CreateExampleCommand(string.Empty, new DateTime(2013, 1, 7));

        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);

        actual.ShouldHaveOneErrorWithMessage(nameof(CreateExampleCommand.Name), ValidationMessages.ValueRequired);
    }
    
    [Fact]
    public async Task CreateExampleCommandValidator_FailsValidation_WhenNameTooLong()
    {
        var command = new CreateExampleCommand(new string('-',  Constants.NameMaximumLength + 1), new DateTime(2013, 1, 7));

        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);

        actual.ShouldHaveOneErrorWithMessage(nameof(CreateExampleCommand.Name), ValidationMessages.ValueExceedsMaximumLengthOf(Constants.NameMaximumLength));
    }

    [Fact]
    public async Task CreateExampleCommandValidator_FailsValidation_WhenNameIsDuplicate()
    {
        var name = Example.SeedData.First().Name;
        var command = new CreateExampleCommand(name, new DateTime(2013, 1, 7));

        var sut = CreateSut();
        var actual = await sut.ValidateAsync(command);

        actual.ShouldHaveOneErrorWithMessage(nameof(CreateExampleCommand.Name), ValidationMessages.ValueMustBeUnique);
    }
    
    // Private Methods

    private CreateExampleCommandValidator CreateSut() 
        => new();
}