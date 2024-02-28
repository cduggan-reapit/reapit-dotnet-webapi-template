using FluentValidation;
using FluentValidation.Results;
using Reapit.Services.Template.Core.UseCases.Examples.GetExamples;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Core.UnitTests.UseCases.Examples.GetExamples;

public class GetExamplesQueryHandlerTests
{
    private readonly IValidator<GetExamplesQuery> _validator = Substitute.For<IValidator<GetExamplesQuery>>();
    
    [Fact]
    public async Task Handle_ShouldReturnPagedFromCollection()
    {
        _validator.ValidateAsync(Arg.Any<GetExamplesQuery>())
            .Returns(new ValidationResult());
        
        var expected = Example.SeedData.Skip(2).Take(2);
        var sut = CreateSut();
        var actual = await sut.Handle(new GetExamplesQuery(2, 2), default);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenValidationFailed()
    {
        _validator.ValidateAsync(Arg.Any<GetExamplesQuery>())
            .Returns(new ValidationResult(new[] { new ValidationFailure("propertyName", "errorMessage") }));

        var sut = CreateSut();
        var action = () => sut.Handle(new GetExamplesQuery(1, 25), default);
        await action.Should().ThrowAsync<ValidationException>();
    }
    
    // Private Methods

    private GetExamplesQueryHandler CreateSut()
        => new(_validator);
}