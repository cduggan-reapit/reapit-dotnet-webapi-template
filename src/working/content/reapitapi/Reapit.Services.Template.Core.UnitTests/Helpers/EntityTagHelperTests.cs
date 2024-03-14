using Reapit.Services.Template.Core.Helpers;
using Reapit.Services.Template.Domain.Entities;

namespace Reapit.Services.Template.Core.UnitTests.Helpers;

public class EntityTagHelperTests
{
    [Fact]
    public void GetEtag_ReturnsEtag_ForIBaseEntity()
    {
        const string expectedEtag = "\"215CEDD750F9CAB40E40567D2B2981F5\"";
        
        var passingEntity = new TestBaseEntityImplementation("Test", DateTimeOffset.UnixEpoch, DateTimeOffset.MaxValue);
        passingEntity.GetEtag().Should().BeEquivalentTo(expectedEtag);
    }
    
    /// <summary>
    /// Test implementation of <see cref="IBaseEntity"/> for use in testing <see cref="EntityTagHelper.GetEtag"/>
    /// </summary>
    /// <param name="Id">The identifier of the entity</param>
    /// <param name="Created">The creation date</param>
    /// <param name="Modified">The last modification date</param>
    private record TestBaseEntityImplementation(string Id, DateTimeOffset Created, DateTimeOffset Modified)
        : IBaseEntity;
}