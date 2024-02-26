namespace Reapit.Services.Template.Domain.Providers;

/// <summary>
/// Provider for DateTimeOffsets accounting for ambient context
/// </summary>
public static class SystemTimeProvider
{
    /// <summary>
    /// The current context DateTimeOffset
    /// </summary>
    public static DateTimeOffset Now
        => SystemTimeProviderContext.Current == null
            ? DateTimeOffset.Now
            : SystemTimeProviderContext.Current.ContextDateTimeNow;
}