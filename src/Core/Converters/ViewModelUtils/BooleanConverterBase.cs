namespace Shipwreck.ViewModelUtils;

public abstract class BooleanConverterBase : IValueConverter
{
#if IS_WPF
    private static readonly object _Visible = Visibility.Visible;
    private static readonly object _Collapsed = Visibility.Collapsed;
#endif
    protected BooleanConverterBase()
    {
#if IS_WPF
        TruePart = FalsePart = DependencyProperty.UnsetValue;
#else
            TruePart = BooleanBoxes.True;
            FalsePart = BooleanBoxes.False;
#endif
    }

    public object TruePart { get; set; }
    public object FalsePart { get; set; }

    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();

    protected object ToResult(bool value, Type targetType, CultureInfo culture)
        => ToResultCore(value, value ? TruePart : FalsePart, targetType, culture);

    internal static object ToResultCore(bool value, object v, Type targetType, CultureInfo culture)
    {
#if IS_WPF
        if (v == DependencyProperty.UnsetValue)
        {
            if (targetType == typeof(Visibility))
            {
                return value ? _Visible : _Collapsed;
            }
            return value ? BooleanBoxes.True : BooleanBoxes.False;
        }
#endif
        return v;
    }
}
