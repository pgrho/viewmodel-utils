namespace Shipwreck.ViewModelUtils
{
    public sealed class GreaterThanConverter : ComparisonConverterBase
    {
        protected override bool ToBoolean(int sign) => sign > 0;
    }
}
