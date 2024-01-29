namespace Shipwreck.ViewModelUtils;

[ValueConversion(typeof(double), typeof(double))]
public abstract class ArithmeticConverter : IValueConverter
{
    public abstract double Operand { get; set; }

    // TODO other numeric targetType
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => Calculate((value as IConvertible)?.ToDouble(culture) ?? double.NaN, Operand);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();

    protected abstract double Calculate(double x, double y);
}
