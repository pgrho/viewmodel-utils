namespace Shipwreck.ViewModelUtils;

public abstract class CommandBuilderBase
{
    public string Title { get; set; }
    public Func<string> TitleGetter { get; set; }
    public string Mnemonic { get; set; }
    public Func<string> MnemonicGetter { get; set; }
    public string Description { get; set; }
    public Func<string> DescriptionGetter { get; set; }
    public bool? IsVisible { get; set; }
    public Func<bool> IsVisibleGetter { get; set; }
    public bool? IsEnabled { get; set; }
    public Func<bool> IsEnabledGetter { get; set; }
    public string Icon { get; set; }
    public Func<string> IconGetter { get; set; }
    public BorderStyle? Style { get; set; }
    public Func<BorderStyle> StyleGetter { get; set; }
    public int? BadgeCount { get; set; }
    public Func<int> BadgeCountGetter { get; set; }
    public string Href { get; set; }
    public Func<string> HrefGetter { get; set; }

    public abstract CommandViewModelBase Build();
}
