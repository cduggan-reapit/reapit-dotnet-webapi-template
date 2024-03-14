using Reapit.Services.Template.Core.Helpers;

namespace Reapit.Services.Template.Core.UnitTests.Helpers;

public class ChecksumHelperTests
{
    [Fact]
    public void GetHashValue_ReturnsMd5Hash_ForInputString()
    {
        const string input = "Hello, world!";
        const string expected = "6cd3556deb0da54bca060b4c39479839";

        var hash = ChecksumHelper.GetHashValue(input);
        hash.Should().BeEquivalentTo(expected);
    }
}