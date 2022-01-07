namespace Shipwreck.ViewModelUtils;

public sealed class NullToFalseConverter : BooleanConverterBase
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => ToResult(value != null, targetType, culture);
}
