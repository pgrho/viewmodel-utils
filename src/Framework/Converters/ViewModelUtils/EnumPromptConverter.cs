namespace Shipwreck.ViewModelUtils;

#if IS_WPF
    [ValueConversion(typeof(Enum), typeof(string))]
#endif
public class EnumPromptConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is Enum ? EnumDataAnnotations.Get(value.GetType()).GetPrompt(value) : value;

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
