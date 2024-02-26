using FluentValidation;
using FluentValidation.Results;
using Reapit.Services.Template.Core.UseCases.Examples.CreateExample;
using Reapit.Services.Template.Domain.Providers;

namespace Reapit.Services.Template.Core.UnitTests.UseCases.Examples.CreateExample;

public class CreateExampleCommandHandlerTests
{
    private readonly IValidator<CreateExampleCommand> _validator = Substitute.For<IValidator<CreateExampleCommand>>();

    private static readonly DateTimeOffset FixedDate = new DateTimeOffset(1997, 9, 5, 11, 32, 0, TimeSpan.Zero);
    
    // Handle

    [Fact]
    public async Task Handle_ReturnsExample_WhenExampleCreated()
    {
        using var timeContext = new SystemTimeProviderContext(FixedDate);
        
        _validator.ValidateAsync(Arg.Any<CreateExampleCommand>())
            .Returns(new ValidationResult());

        var command = new CreateExampleCommand("Name", DateTime.UnixEpoch);
        
        var sut = CreateSut();
        var actual = await sut.Handle(command, default);

        actual.Name.Should().BeEquivalentTo(command.Name);
        actual.Date.Should().Be(command.Date);
        actual.Created.Should().Be(FixedDate);
        actual.Modified.Should().Be(FixedDate);
    }
    
    [Fact]
    public async Task Handle_ThrowsValidationException_WhenValidationFailed()
    {
        using var timeContext = new SystemTimeProviderContext(FixedDate);
        
        _validator.ValidateAsync(Arg.Any<CreateExampleCommand>())
            .Returns(new ValidationResult(new ValidationFailure[]{ new ("example", "error") }));

        var command = new CreateExampleCommand("Name", DateTime.UnixEpoch);
        
        var sut = CreateSut();
        var action = () => sut.Handle(command, default);

        await action.Should().ThrowAsync<ValidationException>();
    }
    
    // Private Methods

    private CreateExampleCommandHandler CreateSut()
        => new CreateExampleCommandHandler(_validator);
}