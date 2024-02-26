using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Reapit.Services.Template.Api.Models.Examples;

/// <summary>
/// Automapper profile for <see cref="Example"/> objects
/// </summary>
public class ExampleProfile : Profile
{
    /// <summary>
    /// Initialize a new instance of the <see cref="ExampleProfile"/> class
    /// </summary>
    public ExampleProfile()
    {
        CreateMap<Domain.Entities.Examples.Example, ExampleReadDto>()
            .ForCtorParam(nameof(ExampleReadDto.Id), ops => ops.MapFrom(entity => entity.Id))
            .ForCtorParam(nameof(ExampleReadDto.Date), ops => ops.MapFrom(entity => entity.Date))
            .ForCtorParam(nameof(ExampleReadDto.Name), ops => ops.MapFrom(entity => entity.Name))
            .ForCtorParam(nameof(ExampleReadDto.DateCreated), ops => ops.MapFrom(entity => entity.Created))
            .ForCtorParam(nameof(ExampleReadDto.DateLastModified), ops => ops.MapFrom(entity => entity.Modified));

        CreateMap<JsonPatchDocument<ExampleWriteDto>, JsonPatchDocument<Domain.Entities.Examples.Example>>();
        CreateMap<Operation<ExampleWriteDto>, Operation<Domain.Entities.Examples.Example>>();
    }
}