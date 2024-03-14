using Reapit.Services.Template.Api.Controllers.Abstract;

namespace Reapit.Services.Template.Api.UnitTests.TestHelpers;

/// <summary>
/// Extension class containing helper methods for use in testing implementations of <see cref="BaseController"/>
/// </summary>
public static class BaseControllerTestExtensions
{
    /// <summary>
    /// Get the ETag header value from the controller context
    /// </summary>
    /// <param name="sut">The system under test</param>
    public static string? GetEtag(this BaseController sut)
        => sut.ControllerContext.HttpContext.Response.Headers.ETag;
}