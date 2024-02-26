using Microsoft.AspNetCore.Mvc;

namespace Reapit.Services.Template.Api.Controllers.Abstract;

/// <summary>
/// Abstract controller defining methods common to all controllers 
/// </summary>
[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Set the "ETag" header in the HTTP response
    /// </summary>
    /// <param name="etag">The entity tag</param>
    internal void SetEtagHeader(string etag)
        => HttpContext.Response.Headers.ETag = etag;
}