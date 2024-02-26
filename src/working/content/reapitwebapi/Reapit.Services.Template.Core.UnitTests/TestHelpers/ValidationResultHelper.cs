using FluentValidation.Results;

namespace Reapit.Services.Template.Core.UnitTests.TestHelpers;

/// <summary>
/// Extension methods for the <see cref="ValidationResult"/> class
/// </summary>
public static class ValidationResultHelper
{
    /// <summary>
    /// Checks that a <see cref="ValidationResult"/> contains one error for given property with a given message
    /// </summary>
    /// <param name="result">The validation result</param>
    /// <param name="expectedProperty">The property name expected to have an error</param>
    /// <param name="expectedMessage">The error message expected to be associated with the property</param>
    public static void ShouldHaveOneErrorWithMessage(this ValidationResult result, string expectedProperty, string expectedMessage)
    {
        result.Errors.Should().HaveCount(1);

        var error = result.Errors.Single();
        error.PropertyName.Should().BeEquivalentTo(expectedProperty);
        error.ErrorMessage.Should().BeEquivalentTo(expectedMessage);
    }
}