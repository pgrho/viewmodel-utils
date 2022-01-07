namespace Shipwreck.ViewModelUtils;

public sealed class LessThanOrEqualConverter : ComparisonConverterBase
{
    protected override bool ToBoolean(int sign) => sign <= 0;
}
