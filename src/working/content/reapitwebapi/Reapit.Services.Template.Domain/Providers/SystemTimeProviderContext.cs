namespace Reapit.Services.Template.Domain.Providers;

/// <summary>
/// Ambient context to enable fixing DateTimeOffsets in test scenarios
/// </summary>
public class SystemTimeProviderContext : IDisposable
{
    internal DateTimeOffset ContextDateTimeNow;
    
    private static readonly ThreadLocal<Stack<SystemTimeProviderContext>> ThreadScopeStack = new(() => new Stack<SystemTimeProviderContext>());
    
    /// <summary>
    /// Initialize a new instance of <see cref="SystemTimeProviderContext"/>
    /// </summary>
    /// <param name="contextDateTimeNow">The fixed date time of the context</param>
    public SystemTimeProviderContext(DateTimeOffset contextDateTimeNow)
    {
        ContextDateTimeNow = contextDateTimeNow;
        ThreadScopeStack.Value?.Push(this);
    }

    /// <summary>
    /// The current context. Returns null when no context configured.
    /// </summary>
    public static SystemTimeProviderContext? Current 
        => ThreadScopeStack.Value?.Count == 0 ? null : ThreadScopeStack.Value?.Peek();
    
    /// <inheritdoc />
    public void Dispose()
    {
        ThreadScopeStack.Value?.Pop();
        GC.SuppressFinalize(this);
    }
}