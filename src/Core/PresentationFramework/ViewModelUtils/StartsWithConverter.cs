namespace Shipwreck.ViewModelUtils;

public sealed class StartsWithConverter : StringContainsConverterBase
{
    protected override bool Compare(string first, string other, CultureInfo culture)
        => culture.CompareInfo.IsPrefix(first, other, CompareOptions);
}
