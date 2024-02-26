using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Reapit.Packages.ErrorHandling.Errors;
using Reapit.Packages.ErrorHandling.Exceptions;
using Reapit.Services.Template.Api.Controllers.Abstract;
using Reapit.Services.Template.Api.Infrastructure.Swagger.RequestExamples;
using Reapit.Services.Template.Api.Infrastructure.Swagger.ResponseExamples;
using Reapit.Services.Template.Api.Models.Examples;
using Reapit.Services.Template.Core.Helpers;
using Reapit.Services.Template.Core.UseCases.Examples.CreateExample;
using Reapit.Services.Template.Core.UseCases.Examples.DeleteExample;
using Reapit.Services.Template.Core.UseCases.Examples.GetExampleById;
using Reapit.Services.Template.Core.UseCases.Examples.GetExamples;
using Reapit.Services.Template.Core.UseCases.Examples.PatchExample;
using Reapit.Services.Template.Core.UseCases.Examples.UpdateExample;
using Reapit.Services.Template.Domain.Entities.Examples;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Template.Api.Controllers;

/// <summary>
/// An example endpoint from the Reapit.Services.Template project
/// </summary>
public class ExampleController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<ExampleController> _logger;

    /// <summary>
    /// Initialize a new instance of the <see cref="ExampleController"/> class
    /// </summary>
    /// <param name="mediator">The mediator service</param>
    /// <param name="mapper">The automapper mapping service</param>
    /// <param name="logger">The application logger</param>
    public ExampleController(IMediator mediator, IMapper mapper, ILogger<ExampleController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Fetch a paged list of all Examples
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve</param>
    /// <param name="pageSize">The number of items per page</param>
    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ExampleReadDtoCollectionExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<IEnumerable<ExampleReadDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationErrorModel>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ApplicationErrorModel>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllExamples(int pageNumber = 1, int pageSize = 25)
    {
        try
        {
            var entities = await _mediator.Send(new GetExamplesQuery(pageNumber, pageSize));
            
            if (!entities.Any())
                return NoContent();
            
            var models = _mapper.Map<IEnumerable<ExampleReadDto>>(entities);
            return Ok(models);
        }
        catch (ValidationException ex)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, ValidationErrorModel.FromException(ex));
        }
    }
    
    /// <summary>
    /// Get an Example by its identifier
    /// </summary>
    /// <param name="id">Unique identifier of the Example</param>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ExampleReadDtoCollectionExample))]
    [ProducesResponseType<ExampleReadDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<NotFoundErrorModel>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApplicationErrorModel>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetExampleById(string id)
    {
        try
        {
            var entity = await _mediator.Send(new GetExampleByIdQuery(id));
            
            SetEtagHeader(entity.GetEtag());
            
            var model = _mapper.Map<ExampleReadDto>(entity);
            return Ok(model);
        }
        catch (NotFoundException ex)
        {
            return StatusCode(StatusCodes.Status404NotFound, NotFoundErrorModel.FromException(ex));
        }
    }

    /// <summary>
    /// Create a new Example
    /// </summary>
    /// <param name="model">The Example to create</param>
    [HttpPost]
    [SwaggerRequestExample(typeof(ExampleWriteDto), typeof(ExampleWriteDtoExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ExampleReadDtoCollectionExample))]
    [ProducesResponseType<ExampleReadDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationErrorModel>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ApplicationErrorModel>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateExample([FromBody] ExampleWriteDto model)
    {
        try
        {
            var command = new CreateExampleCommand(model.Name, model.Date);
            var entity = await _mediator.Send(command);

            SetEtagHeader(entity.GetEtag());
            
            var readModel = _mapper.Map<ExampleReadDto>(entity);
            return CreatedAtAction(nameof(GetExampleById), new { id = entity.Id }, readModel);
        }
        catch (ValidationException ex)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, ValidationErrorModel.FromException(ex));
        }
    }
    
    /// <summary>
    /// Update an existing Example
    /// </summary>
    /// <param name="id">Unique identifier of the Example</param>
    /// <param name="model">The new properties of the Example</param>
    /// <param name="etag">The entity tag of the Example</param>
    [HttpPut("{id}")]
    [SwaggerRequestExample(typeof(ExampleWriteDto), typeof(ExampleWriteDtoExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<NotFoundErrorModel>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ConflictErrorModel>(StatusCodes.Status412PreconditionFailed)]
    [ProducesResponseType<ValidationErrorModel>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ApplicationErrorModel>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateExample(string id, [FromBody] ExampleWriteDto model, [FromHeader(Name = "If-Match")] string etag)
    {
        try
        {
            var command = new UpdateExampleCommand(id, model.Name, model.Date, etag);
            var entity = await _mediator.Send(command);

            SetEtagHeader(entity.GetEtag());

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, ValidationErrorModel.FromException(ex));
        }
        catch (NotFoundException ex)
        {
            return StatusCode(StatusCodes.Status404NotFound, NotFoundErrorModel.FromException(ex));
        }
        catch (ConflictException ex)
        {
            return StatusCode(StatusCodes.Status412PreconditionFailed, ConflictErrorModel.FromException(ex));
        }
    }
    
    /// <summary>
    /// Delete an Example
    /// </summary>
    /// <param name="id">Unique identifier of the Example</param>
    /// <param name="etag">The entity tag of the Example</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<NotFoundErrorModel>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ConflictErrorModel>(StatusCodes.Status412PreconditionFailed)]
    [ProducesResponseType<ApplicationErrorModel>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteExample(string id, [FromHeader(Name = "If-Match")] string etag)
    {
        try
        {
            var command = new DeleteExampleCommand(id, etag);
            await _mediator.Send(command);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return StatusCode(StatusCodes.Status404NotFound, NotFoundErrorModel.FromException(ex));
        }
        catch (ConflictException ex)
        {
            return StatusCode(StatusCodes.Status412PreconditionFailed, ConflictErrorModel.FromException(ex));
        }
    }

    /// <summary>
    /// Apply a set of changes to an Example
    /// </summary>
    /// <param name="id">Unique identifier of the Example</param>
    /// <param name="patchDocument">The changes to apply to the Example</param>
    /// <param name="etag">The entity tag of the Example</param>
    [HttpPatch("{id}")]
    [SwaggerRequestExample(typeof(JsonPatchDocument<ExampleWriteDto>), typeof(ExampleWriteDtoPatchExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<NotFoundErrorModel>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ConflictErrorModel>(StatusCodes.Status412PreconditionFailed)]
    [ProducesResponseType<ValidationErrorModel>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ApplicationErrorModel>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PatchExample(string id, [FromBody] JsonPatchDocument<ExampleWriteDto> patchDocument, [FromHeader(Name = "If-Match")] string etag)
    {
        try
        {
            var entityPatchDocument = _mapper.Map<JsonPatchDocument<Example>>(patchDocument);
            
            var command = new PatchExampleCommand(id, etag, entityPatchDocument);
            var updated = await _mediator.Send(command);
            
            SetEtagHeader(updated.GetEtag());
            
            return NoContent();
        }
        catch (ValidationException ex)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, ValidationErrorModel.FromException(ex));
        }
        catch (NotFoundException ex)
        {
            return StatusCode(StatusCodes.Status404NotFound, NotFoundErrorModel.FromException(ex));
        }
        catch (ConflictException ex)
        {
            return StatusCode(StatusCodes.Status412PreconditionFailed, ConflictErrorModel.FromException(ex));
        }
    }
}