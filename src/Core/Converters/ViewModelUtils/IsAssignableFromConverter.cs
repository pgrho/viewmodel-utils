namespace Shipwreck.ViewModelUtils;

public sealed class IsAssignableFromConverter : BooleanConverterBase
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => ToResult(value != null && parameter is Type pt && pt.IsAssignableFrom(value.GetType()), targetType, culture);
}
