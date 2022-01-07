namespace Shipwreck.ViewModelUtils;

public sealed class EndsWithConverter : StringContainsConverterBase
{
    protected override bool Compare(string first, string other, CultureInfo culture)
        => culture.CompareInfo.IsSuffix(first, other, CompareOptions);
}
