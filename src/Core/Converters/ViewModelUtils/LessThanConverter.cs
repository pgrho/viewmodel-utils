namespace Shipwreck.ViewModelUtils
{
    public sealed class LessThanConverter : ComparisonConverterBase
    {
        protected override bool ToBoolean(int sign) => sign < 0;
    }
}
