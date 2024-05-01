using System.Globalization;
using DevToys.Tools.Helpers;
using Microsoft.Extensions.Logging;

namespace DevToys.Tools.Tools.Generators.Fake;

[Export(typeof(ICommandLineTool))]
[Name(internalComponentName: "LoremIpsumGenerator")]
[CommandName(
    Name = "faker",
    Alias = "fd",
    ResourceManagerBaseName = "DevToys.Tools.Tools.Generators.Fake.FakeDataGenerator",
    DescriptionResourceName = nameof(FakeDataGenerator.Description))]
internal sealed class FakeDataGeneratorCommandLineTool : ICommandLineTool
{
    [CommandLineOption(
        Name = "count",
        Alias = "c",
        DescriptionResourceName = nameof(FakeDataGenerator.CountDescription))]
    internal int Count { get; set; } = 5;

    [CommandLineOption(
        Name = "type",
        Alias = "t",
        DescriptionResourceName = nameof(FakeDataGenerator.StringFakeTypeDescription))]
    internal string Type { get; set; } = "phone";

    public ValueTask<int> InvokeAsync(ILogger logger, CancellationToken cancellationToken)
    {
        FakeHelper.SetLocale(CultureInfo.CurrentCulture.Name);

        for (int i = 0; i < Count; i++)
        {
            switch (Type)
            {
                case "phone":
                    {
                        string phone = FakeHelper.GetPhoneNumber();
                        Console.WriteLine(phone);
                        break;
                    }
                case "email":
                    {
                        string emailAddress = FakeHelper.GetEmailAddress();
                        Console.WriteLine(emailAddress);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return ValueTask.FromResult(0);
    }
}
