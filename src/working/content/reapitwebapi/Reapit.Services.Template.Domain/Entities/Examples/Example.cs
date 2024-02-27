using Reapit.Services.Template.Domain.Providers;

namespace Reapit.Services.Template.Domain.Entities.Examples;

public class Example : IBaseEntity
{
    /// <summary>
    /// Initialize a new instance of <see cref="Example"/>
    /// </summary>
    /// <remarks>The parameterless constructor is internal and should only be used for unit tests</remarks> 
    internal Example()
    {
    }

    /// <summary>
    /// Initialize a new instance of <see cref="Example"/>
    /// </summary>
    /// <param name="name">The name of the example</param>
    /// <param name="date">The date of the example</param>
    private Example(string name, DateTime date)
    {
        Id = Guid.NewGuid().ToString("N");
        Name = name;
        Date = date;
        Created = SystemTimeProvider.Now;
        Modified = SystemTimeProvider.Now;
    }
    
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public DateTime Date { get; set; }
    
    public DateTimeOffset Created { get; set; }
    
    public DateTimeOffset Modified { get; set; }

    public static Example Create(string name, DateTime date)
        => new (name, date);
    
    /// <summary>
    /// Placeholder collection to allow example queries and commands to read a collection
    /// </summary>
    /// <remarks>
    /// This is just placeholder data to let us test the endpoints.  This should be replaced with a data layer or api definition.
    /// </remarks>
    public static readonly Example[] SeedData =
    [
        new Example { Id = "10ac6339-23f8-42c0-87c0-8d7f8980f36a", Name = "Sarina Emely", Date = new DateTime(1990, 2, 7), Created = SystemTimeProvider.Now, Modified = SystemTimeProvider.Now },
        new Example { Id = "60e4ecb5-2cd9-457e-8de9-305c3a8d886e", Name = "Steph Lone", Date = new DateTime(1990, 1, 26), Created = SystemTimeProvider.Now, Modified = SystemTimeProvider.Now },
        new Example { Id = "213a66d7-1af0-4a0e-9136-ad350afbe445", Name = "Pearce Harold", Date = new DateTime(1991, 9, 5), Created = SystemTimeProvider.Now, Modified = SystemTimeProvider.Now },
        new Example { Id = "fb1ce86d-fbfc-4d58-800a-03640b669fc5", Name = "Darren Graeme", Date = new DateTime(1967, 9, 11), Created = SystemTimeProvider.Now, Modified = SystemTimeProvider.Now }
    ];
}