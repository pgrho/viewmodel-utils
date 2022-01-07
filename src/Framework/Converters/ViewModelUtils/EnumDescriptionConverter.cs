namespace Shipwreck.ViewModelUtils;

#if IS_WPF
    [ValueConversion(typeof(Enum), typeof(string))]
#endif
public class EnumDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is Enum ? EnumDataAnnotations.Get(value.GetType()).GetDescription(value) : value;

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
