using Reapit.Services.Template.Domain.Providers;

namespace Reapit.Services.Template.Domain.UnitTests.Providers;

public class SystemTimeProviderTests
{
    [Fact]
    public void Now_ReturnsNow_WhenNoContextConfiguredForScope()
        => SystemTimeProvider.Now.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));

    [Fact]
    public void Now_ReturnsConfiguredTime_WhenContextConfiguredForScope()
    {
        var fixedDate = new DateTimeOffset(2016, 4, 16, 7, 53, 12, TimeSpan.FromHours(-5));
        using var timeContext = new SystemTimeProviderContext(fixedDate);
        SystemTimeProvider.Now.Should().Be(fixedDate);
    }
}