namespace Shipwreck.ViewModelUtils;

public sealed class StringContainsConverter : StringContainsConverterBase
{
    protected override bool Compare(string first, string other, CultureInfo culture)
        => culture.CompareInfo.IndexOf(first, other, CompareOptions) >= 0;
}
