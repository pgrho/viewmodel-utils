namespace Shipwreck.ViewModelUtils;

public abstract class StringContainsConverterBase : BooleanConverterBase, IMultiValueConverter
{
    public CompareOptions CompareOptions { get; set; } = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreWidth;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => ToResult(CompareCore(values, culture), targetType, culture);

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => ToResult(
            value is string fs
            && (parameter == null
                || parameter is string o && (string.IsNullOrWhiteSpace(o) || Compare(fs, o.Trim(), culture))), targetType, culture);

    private bool CompareCore(object[] values, CultureInfo culture)
    {
        var fs = values.FirstOrDefault() as string;
        if (fs == null)
        {
            return false;
        }
        foreach (var e in values.Skip(1))
        {
            if (!(e == null || e is string s && (string.IsNullOrWhiteSpace(s) || Compare(fs, s.Trim(), culture))))
            {
                return false;
            }
        }
        return true;
    }

    protected abstract bool Compare(string first, string other, CultureInfo culture);

    object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => Enumerable.Repeat(Binding.DoNothing, targetTypes.Length).ToArray();
}
