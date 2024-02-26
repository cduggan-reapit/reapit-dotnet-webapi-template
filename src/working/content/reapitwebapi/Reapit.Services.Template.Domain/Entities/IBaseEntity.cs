namespace Reapit.Services.Template.Domain.Entities;

public interface IBaseEntity
{
    public string Id { get; }
    
    public DateTimeOffset Created { get; }
    
    public DateTimeOffset Modified { get; }
}