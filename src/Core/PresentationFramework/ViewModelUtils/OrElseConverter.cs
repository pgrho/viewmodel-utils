namespace Shipwreck.ViewModelUtils;

public sealed class OrElseConverter : IMultiValueConverter
{
    public object TruePart { get; set; } = DependencyProperty.UnsetValue;
    public object FalsePart { get; set; } = DependencyProperty.UnsetValue;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var v = values.Any(e => e != null && (!(e is bool b) || b));
        return BooleanConverterBase.ToResultCore(v, v ? TruePart : FalsePart, targetType, culture);
    }

    object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
