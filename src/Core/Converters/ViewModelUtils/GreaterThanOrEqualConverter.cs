namespace Shipwreck.ViewModelUtils;

public sealed class GreaterThanOrEqualConverter : ComparisonConverterBase
{
    protected override bool ToBoolean(int sign) => sign >= 0;
}
