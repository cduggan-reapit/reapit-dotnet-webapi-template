using Reapit.Services.Template.Core.UnitTests.TestHelpers;
using Reapit.Services.Template.Core.UseCases;
using Reapit.Services.Template.Core.UseCases.Examples.GetExamples;

namespace Reapit.Services.Template.Core.UnitTests.UseCases.Examples.GetExamples;

public class GetExamplesQueryValidatorTests
{
    [Fact]
    public async Task Validation_ShouldPass_WhenQueryValid()
    {
        var query = new GetExamplesQuery(1, 25);
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(query);
        actual.IsValid.Should().BeTrue();
    }
    
    // PageNumber

    [Fact]
    public async Task Validation_ShouldFail_WhenPageNumberZero()
    {
        var query = new GetExamplesQuery(0, 25);
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(query);
        actual.ShouldHaveOneErrorWithMessage(nameof(GetExamplesQuery.PageNumber), ValidationMessages.ValueMustBeGreaterThanZero);
    }
    
    // PageSize
    
    [Fact]
    public async Task Validation_ShouldFail_WhenPageSizeZero()
    {
        var query = new GetExamplesQuery(1, 0);
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(query);
        actual.ShouldHaveOneErrorWithMessage(nameof(GetExamplesQuery.PageSize), ValidationMessages.ValueMustBeGreaterThanZero);
    }
    
    [Fact]
    public async Task Validation_ShouldFail_WhenPageSizeGreaterThanLimit()
    {
        var query = new GetExamplesQuery(1, QueryConstants.PageSizeLimit + 1);
        var sut = CreateSut();
        var actual = await sut.ValidateAsync(query);
        actual.ShouldHaveOneErrorWithMessage(nameof(GetExamplesQuery.PageSize), ValidationMessages.ValueExceedsMaximumOf(QueryConstants.PageSizeLimit));
    }
    
    // Private Methods

    private GetExamplesQueryValidator CreateSut()
        => new();
}