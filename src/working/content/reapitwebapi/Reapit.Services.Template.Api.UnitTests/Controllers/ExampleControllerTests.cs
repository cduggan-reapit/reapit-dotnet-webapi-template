using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;
using Reapit.Services.Template.Api.Controllers;
using Reapit.Services.Template.Api.Models.Examples;
using Reapit.Services.Template.Core.UseCases.Examples.GetExamples;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Reapit.Packages.ErrorHandling.Exceptions;
using Reapit.Packages.ErrorHandling.Providers;
using Reapit.Services.Template.Api.UnitTests.TestHelpers;
using Reapit.Services.Template.Core.Helpers;
using Reapit.Services.Template.Core.UseCases.Examples.CreateExample;
using Reapit.Services.Template.Core.UseCases.Examples.DeleteExample;
using Reapit.Services.Template.Core.UseCases.Examples.GetExampleById;
using Reapit.Services.Template.Core.UseCases.Examples.PatchExample;
using Reapit.Services.Template.Core.UseCases.Examples.UpdateExample;
using Reapit.Services.Template.Domain.Entities.Examples;

namespace Reapit.Services.Template.Api.UnitTests.Controllers;

public class ExampleControllerTests
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly ILogger<ExampleController> _logger = Substitute.For<ILogger<ExampleController>>();
    private static readonly DateTimeOffset FixedTimestamp = new (2024, 2, 26, 14, 3, 43, TimeSpan.Zero);
    
    public ExampleControllerTests()
    {
        _mapper = new MapperConfiguration(cfg => 
                cfg.AddProfile<ExampleProfile>())
            .CreateMapper();
    }
    
    // GetAllExamples

    [Fact]
    public async Task GetAllExamples_ReturnsUnprocessableEntity_WhenValidationFailed()
    {
        _mediator.Send(Arg.Any<GetExamplesQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ValidationException(Array.Empty<ValidationFailure>()));

        var sut = CreateSut();
        var result = await sut.GetAllExamples() as ObjectResult;
        
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(422);
    }

    [Fact]
    public async Task GetAllExamples_ReturnsNoContent_WhenNoDataAvailable()
    {
        _mediator.Send(Arg.Any<GetExamplesQuery>(), Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Example>());

        var sut = CreateSut();
        var result = await sut.GetAllExamples() as NoContentResult;
        
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(204);

        await _mediator.Received(1).Send(Arg.Is<GetExamplesQuery>(actual => actual.PageSize == 25 && actual.PageNumber == 1));
    }

    [Fact]
    public async Task GetAllExamples_ReturnsSuccess_WhenDataAvailable()
    {
        const int pageNumber = 42;
        const int pageSize = 7;
        
        var examples = new[]
        {
            new Example
            {
                Id = "ID1",
                Name = "Test Example #1",
                Date = DateTime.UnixEpoch,
                Created = DateTimeOffset.UnixEpoch,
                Modified = DateTimeOffset.UnixEpoch
            },
            new Example
            {
                Id = "ID2",
                Name = "Test Example #2",
                Date = DateTime.MinValue,
                Created = DateTimeOffset.MinValue,
                Modified = DateTimeOffset.MinValue
            },
            new Example
            {
                Id = "ID3",
                Name = "Test Example #3",
                Date = DateTime.MaxValue,
                Created = DateTimeOffset.MaxValue,
                Modified = DateTimeOffset.MaxValue
            }
        };
        
        var expectedResult = new []
        {
            new ExampleReadDto("ID1", "Test Example #1", DateTime.UnixEpoch, DateTimeOffset.UnixEpoch, DateTimeOffset.UnixEpoch),
            new ExampleReadDto("ID2", "Test Example #2", DateTime.MinValue, DateTimeOffset.MinValue, DateTimeOffset.MinValue),
            new ExampleReadDto("ID3", "Test Example #3", DateTime.MaxValue, DateTimeOffset.MaxValue, DateTimeOffset.MaxValue)
        };

        _mediator.Send(Arg.Any<GetExamplesQuery>(), Arg.Any<CancellationToken>())
            .Returns(examples);

        var sut = CreateSut();
        var result = await sut.GetAllExamples(pageNumber, pageSize) as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
        
        var payload = result.Value as IEnumerable<ExampleReadDto>;
        payload.Should().BeEquivalentTo(expectedResult);

        await _mediator.Received(1).Send(Arg.Is<GetExamplesQuery>(actual => actual.PageNumber == pageNumber && actual.PageSize == pageSize));
    }
    
    // GetExampleById

    [Fact]
    public async Task GetExampleById_ReturnsNotFound_WhenExampleNotFound()
    {
        _mediator.Send(Arg.Any<GetExampleByIdQuery>())
            .ThrowsAsync(x => new NotFoundException(typeof(Example), x.Arg<GetExampleByIdQuery>().Id));

        var sut = CreateSut();
        var result = await sut.GetExampleById("TestId") as ObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(404);
    }
    
    [Fact]
    public async Task GetExampleById_ReturnsSuccess_WhenExampleFound()
    {
        var example = Example.Create("TestId", DateTime.UnixEpoch);
        var expected = _mapper.Map<ExampleReadDto>(example);
        
        _mediator.Send(Arg.Any<GetExampleByIdQuery>())
            .Returns(example);

        var sut = CreateSut();
        var result = await sut.GetExampleById("TestId") as OkObjectResult;
        
        // Check the response code
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);

        // ... payload...
        var content = result.Value as ExampleReadDto;
        content.Should().BeEquivalentTo(expected);

        // ... and etag header
        sut.GetEtag().Should().BeEquivalentTo(example.GetEtag());
    }
    
    // CreateExample

    [Fact]
    public async Task CreateExample_ReturnsUnprocessableEntity_WhenValidationFailed()
    {
        var example = Example.Create("TestExample", new DateTime(2024, 02, 26));
        _mediator.Send(Arg.Any<CreateExampleCommand>())
            .ThrowsAsync(new ValidationException(Array.Empty<ValidationFailure>()));

        var sut = CreateSut();
        var result = await sut.CreateExample(new ExampleWriteDto(example.Name, example.Date)) as ObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(422);
    }
    
    [Fact]
    public async Task CreateExample_ReturnsCreated_WhenExampleCreated()
    {
        using var timeContext = new DateTimeOffsetProviderContext(FixedTimestamp);
        
        var example = Example.Create("TestExample", new DateTime(2024, 02, 26));
        _mediator.Send(Arg.Any<CreateExampleCommand>())
            .Returns(example);

        var sut = CreateSut();
        var result = await sut.CreateExample(new ExampleWriteDto(example.Name, example.Date)) as CreatedAtActionResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(201);
        result.Value.Should().BeEquivalentTo(_mapper.Map<ExampleReadDto>(example));
        result.ActionName.Should().BeEquivalentTo(nameof(ExampleController.GetExampleById));
        result.RouteValues.Should().ContainEquivalentOf(new KeyValuePair<string,string>("id", example.Id));

        sut.GetEtag().Should().BeEquivalentTo(example.GetEtag());
    }
    
    // UpdateExample

    [Fact]
    public async Task UpdateExample_ReturnsNotFound_WhenExampleNotFound()
    {
        _mediator.Send(Arg.Any<UpdateExampleCommand>())
            .ThrowsAsync(new NotFoundException(typeof(Example), string.Empty));
        
        var sut = CreateSut();
        var result = await sut.UpdateExample(string.Empty, new ExampleWriteDto(string.Empty, DateTime.MinValue), string.Empty) as ObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task UpdateExample_ReturnsPreconditionFailed_WhenEtagConflict()
    {
        _mediator.Send(Arg.Any<UpdateExampleCommand>())
            .ThrowsAsync(new ConflictException(typeof(Example), string.Empty));
        
        var sut = CreateSut();
        var result = await sut.UpdateExample(string.Empty, new ExampleWriteDto(string.Empty, DateTime.MinValue), string.Empty) as ObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(412);
    }

    [Fact]
    public async Task UpdateExample_ReturnsUnprocessableEntity_WhenValidationFailed()
    {
        _mediator.Send(Arg.Any<UpdateExampleCommand>())
            .ThrowsAsync(new ValidationException(Array.Empty<ValidationFailure>()));
        
        var sut = CreateSut();
        var result = await sut.UpdateExample(string.Empty, new ExampleWriteDto(string.Empty, DateTime.MinValue), string.Empty) as ObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(422);
    }

    [Fact]
    public async Task UpdateExample_ReturnsNoContent_WhenUpdateSuccessful()
    {
        using var timeContext = new DateTimeOffsetProviderContext(FixedTimestamp);

        var example = Example.Create("TestName", DateTime.UnixEpoch);
        _mediator.Send(Arg.Any<UpdateExampleCommand>())
            .Returns(example);
        
        var sut = CreateSut();
        var result = await sut.UpdateExample(string.Empty, new ExampleWriteDto(string.Empty, DateTime.MinValue), string.Empty) as NoContentResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(204);

        sut.GetEtag().Should().BeEquivalentTo(example.GetEtag());
    }
    
    // DeleteExample
    
    [Fact]
    public async Task DeleteExample_ReturnsNotFound_WhenExampleNotFound()
    {
        _mediator.Send(Arg.Any<DeleteExampleCommand>())
            .ThrowsAsync(new NotFoundException(typeof(Example), string.Empty));
        
        var sut = CreateSut();
        var result = await sut.DeleteExample(string.Empty, string.Empty) as ObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(404);
    }
    
    [Fact]
    public async Task DeleteExample_ReturnsPreconditionFailed_WhenEtagConflict()
    {
        _mediator.Send(Arg.Any<DeleteExampleCommand>())
            .ThrowsAsync(new ConflictException(typeof(Example), string.Empty));
        
        var sut = CreateSut();
        var result = await sut.DeleteExample(string.Empty, string.Empty) as ObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(412);
    }
    
    [Fact]
    public async Task DeleteExample_ReturnsNoContent_WhenDeleteSuccessful()
    {
        var sut = CreateSut();
        var result = await sut.DeleteExample(string.Empty, string.Empty) as NoContentResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(204);
    }
    
    // PatchExample

    [Fact]
    public async Task PatchExample_ReturnsNotFound_WhenExampleNotFound()
    {
        _mediator.Send(Arg.Any<PatchExampleCommand>())
            .ThrowsAsync(new NotFoundException(typeof(Example), string.Empty));

        var sut = CreateSut();
        var response = await sut.PatchExample(string.Empty, new JsonPatchDocument<ExampleWriteDto>(), string.Empty) as ObjectResult;

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(404);
    }
    
    [Fact]
    public async Task PatchExample_ReturnsPreconditionFailed_WhenEtagConflicts()
    {
        _mediator.Send(Arg.Any<PatchExampleCommand>())
            .ThrowsAsync(new ConflictException(typeof(Example), string.Empty));

        var sut = CreateSut();
        var response = await sut.PatchExample(string.Empty, new JsonPatchDocument<ExampleWriteDto>(), string.Empty) as ObjectResult;

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(412);
    }
    
    [Fact]
    public async Task PatchExample_ReturnsUnprocessableEntity_WhenValidationFailed()
    {
        _mediator.Send(Arg.Any<PatchExampleCommand>())
            .ThrowsAsync(new ValidationException(Array.Empty<ValidationFailure>()));

        var sut = CreateSut();
        var response = await sut.PatchExample(string.Empty, new JsonPatchDocument<ExampleWriteDto>(), string.Empty) as ObjectResult;

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(422);
    }
    
    [Fact]
    public async Task PatchExample_ReturnsNoContent_WhenPatchApplied()
    {
        using var timeContext = new DateTimeOffsetProviderContext(FixedTimestamp);

        var example = Example.Create("TestName", DateTime.UnixEpoch);
        _mediator.Send(Arg.Any<PatchExampleCommand>())
            .Returns(example);

        var sut = CreateSut();
        var response = await sut.PatchExample(string.Empty, new JsonPatchDocument<ExampleWriteDto>(), string.Empty) as NoContentResult;

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(204);
        sut.GetEtag().Should().BeEquivalentTo(example.GetEtag());
    }
    
    // Private Methods

    private ExampleController CreateSut()
        => new (_mediator, _mapper, _logger)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
}