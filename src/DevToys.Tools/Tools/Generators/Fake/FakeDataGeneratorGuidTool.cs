using System.Globalization;
using DevToys.Tools.Helpers;

namespace DevToys.Tools.Tools.Generators.Fake;

[Export(typeof(IGuiTool))]
[Name("FakeDataGenerator")]
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",
    IconGlyph = '\uED0C',
    GroupName = PredefinedCommonToolGroupNames.Generators,
    ResourceManagerAssemblyIdentifier = nameof(DevToysToolsResourceManagerAssemblyIdentifier),
    ResourceManagerBaseName = "DevToys.Tools.Tools.Generators.Fake.FakeDataGenerator",
    ShortDisplayTitleResourceName = nameof(FakeDataGenerator.ShortDisplayTitle),
    LongDisplayTitleResourceName = nameof(FakeDataGenerator.LongDisplayTitle),
    DescriptionResourceName = nameof(FakeDataGenerator.Description),
    SearchKeywordsResourceName = nameof(FakeDataGenerator.SearchKeywords),
    AccessibleNameResourceName = nameof(FakeDataGenerator.AccessibleName))]
internal sealed class FakeDataGeneratorGuidTool : IGuiTool
{
    private enum GridColumn
    {
        Stretch
    }

    private enum GridRow
    {
        Settings,
        Results
    }

    private static readonly SettingDefinition<FakeType> fakeType
        = new(
            $"{nameof(FakeDataGeneratorGuidTool)}.{nameof(fakeType)}",
            FakeType.PhoneNumber);

    private static readonly SettingDefinition<int> count
        = new(
            $"{nameof(FakeDataGeneratorGuidTool)}.{nameof(count)}",
            1);

    private readonly ISettingsProvider _settingsProvider;
    private readonly IUIMultiLineTextInput _outputText = MultiLineTextInput();

    [ImportingConstructor]
    public FakeDataGeneratorGuidTool(ISettingsProvider settingsProvider)
    {
        _settingsProvider = settingsProvider;
        FakeHelper.SetLocale(CultureInfo.CurrentCulture.Name);
        OnGenerateButtonClick();
    }

    public UIToolView View
        => new(
            true,
            Grid()
                .ColumnLargeSpacing()
                .RowLargeSpacing()
                .Rows(
                    (GridRow.Settings, Auto),
                    (GridRow.Results, new UIGridLength(1, UIGridUnitType.Fraction)))
                .Columns(
                    (GridColumn.Stretch, new UIGridLength(1, UIGridUnitType.Fraction)))
                .Cells(
                    Cell(
                        GridRow.Settings,
                        GridColumn.Stretch,
                        Stack()
                            .Vertical()
                            .LargeSpacing()
                            .WithChildren(
                                Stack()
                                    .Vertical()
                                    .WithChildren(
                                        Label().Text(FakeDataGenerator.ConfigurationTitle),
                                        Setting()
                                            .Icon("FluentSystemIcons", '\uE178')
                                            .Title(FakeDataGenerator.FakeType)
                                            .Handle(
                                                _settingsProvider,
                                                fakeType,
                                                OnSettingChanged,
                                                Item(FakeDataGenerator.FakePhoneNumber, FakeType.PhoneNumber),
                                                Item(FakeDataGenerator.FakeMailAddress, FakeType.MailAddress)
                                            ),
                                        Setting()
                                            .Icon("FluentSystemIcons", '\uF57D')
                                            .Title(FakeDataGenerator.CountTitle)
                                            .Description(FakeDataGenerator.CountDescription)
                                            .InteractiveElement(
                                                NumberInput()
                                                    .HideCommandBar()
                                                    .Minimum(1)
                                                    .OnValueChanged(OnCountChanged)
                                                    .Value(_settingsProvider.GetSetting(count)))))),
                    Cell(
                        GridRow.Results,
                        GridColumn.Stretch,
                        _outputText
                            .Title(FakeDataGenerator.Output)
                            .ReadOnly()
                            .AlwaysWrap()
                            .CommandBarExtraContent(
                                Button()
                                    .Icon("FluentSystemIcons", '\uF13D')
                                    .Text(FakeDataGenerator.Refresh)
                                    .OnClick(OnGenerateButtonClick)))));

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
    }

    private void OnCountChanged(double value)
    {
        _settingsProvider.SetSetting(count, int.Parse(value.ToString(CultureInfo.InvariantCulture)));
        OnGenerateButtonClick();
    }

    private void OnSettingChanged(FakeType value)
    {
        OnGenerateButtonClick();
    }

    private void OnGenerateButtonClick()
    {
        int lineCount = _settingsProvider.GetSetting(count);
        FakeType type = _settingsProvider.GetSetting(fakeType);
        string output = "";
        for (int i = 0; i < lineCount; i++)
        {
            switch (type)
            {
                case FakeType.PhoneNumber:
                    {
                        string phone = FakeHelper.GetPhoneNumber();
                        output += phone + Environment.NewLine;
                        break;
                    }
                case FakeType.MailAddress:
                    {
                        string emailAddress = FakeHelper.GetEmailAddress();
                        output += emailAddress + Environment.NewLine;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        _outputText.Text(output);
    }
}
