using Reapit.Services.Template.Domain.Entities;

namespace Reapit.Services.Template.Core.Helpers;

/// <summary>
/// Helper methods for generating and comparing etags
/// </summary>
public static class EntityTagHelper
{
    /// <summary>
    /// Generate the entity tag for an instance of <see cref="IBaseEntity"/>
    /// </summary>
    /// <param name="entity">The entity</param>
    public static string GetEtag(this IBaseEntity entity)
    {
        var etag = ChecksumHelper.GetHashValue($"{entity.GetType().Name}+{entity.Id}+{entity.Created:O}");
        etag = ChecksumHelper.GetHashValue($"{etag}+{entity.Modified:o}");
        return $"\"{etag}\"";
    }
}