using DevToys.Tools.Helpers;

namespace DevToys.Tools.UnitTests.Tools.Helpers;

public class FakeHelperTest
{
    [Theory]
    [InlineData("^\\+90-\\d{3}-\\d{3}-\\d{1,2}-\\d{3}$", "tr")]
    [InlineData("^\\d{2,5}-\\d{1,4}-\\d{4}$", "ja-jp")]
    public void GeneratePhone_SetLocale(string regexPattern, string localeText)
    {
        FakeHelper.SetLocale(localeText);
        string phone = FakeHelper.GetPhoneNumber();
        Assert.Matches(regexPattern, phone);
    }

    [Fact]
    public void GeneratePhone_DefaultLocale()
    {
        string phone = FakeHelper.GetPhoneNumber();
        Assert.Matches("^(1-)?(\\(\\d{3}\\)|\\d{3})[.-]?\\d{3}[.-]?\\d{4}( x\\d{3,5})?$", phone);
    }
}
