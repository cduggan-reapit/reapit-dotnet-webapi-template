using FluentValidation;
using FluentValidation.Results;
using Reapit.Packages.ErrorHandling.Exceptions;
using Reapit.Services.Template.Core.Helpers;
using Reapit.Services.Template.Core.UseCases.Examples.UpdateExample;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UnitTests.UseCases.Examples.UpdateExample;

public class UpdateExampleCommandHandlerTests
{
    private readonly IValidator<UpdateExampleCommand> _validator = Substitute.For<IValidator<UpdateExampleCommand>>();

    [Fact]
    public async Task Handle_ThrowsValidationException_WhenValidationFailed()
    {
        _validator.ValidateAsync(Arg.Any<UpdateExampleCommand>())
            .Returns(new ValidationResult(new[] { new ValidationFailure("propertyName", "errorMessage") }));

        var sut = CreateSut();
        var action = () => sut.Handle(new UpdateExampleCommand(string.Empty, string.Empty, DateTime.UnixEpoch,string.Empty), default);
        await action.Should().ThrowAsync<ValidationException>();
    }
    
    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenExampleNotExists()
    {
        _validator.ValidateAsync(Arg.Any<UpdateExampleCommand>())
                    .Returns(new ValidationResult());
        
        var sut = CreateSut();
        var action = () => sut.Handle(new UpdateExampleCommand(string.Empty, string.Empty, DateTime.UnixEpoch,string.Empty), default);
        await action.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task Handle_ThrowsConflictException_WhenEtagInvalid()
    {
        _validator.ValidateAsync(Arg.Any<UpdateExampleCommand>())
            .Returns(new ValidationResult());

        var example = Example.SeedData.First();
        
        var sut = CreateSut();
        var action = () => sut.Handle(new UpdateExampleCommand(example.Id , string.Empty, DateTime.UnixEpoch,"invalid"), default);
        await action.Should().ThrowAsync<ConflictException>();
    }
    
    [Fact]
    public async Task Handle_ReturnsUpdatedExample_WhenSuccessfullyApplied()
    {
        _validator.ValidateAsync(Arg.Any<UpdateExampleCommand>())
            .Returns(new ValidationResult());

        var example = Example.SeedData.First();
        
        var sut = CreateSut();
        var actual = await sut.Handle(new UpdateExampleCommand(example.Id , "newName", DateTime.UnixEpoch, example.GetEtag()), default);

        actual.Name.Should().BeEquivalentTo("newName");
        actual.Date.Should().Be(DateTime.UnixEpoch);
    }
    
    // Private Methods

    private UpdateExampleCommandHandler CreateSut()
        => new(_validator);
}