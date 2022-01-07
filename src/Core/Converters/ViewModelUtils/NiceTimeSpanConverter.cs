namespace Shipwreck.ViewModelUtils;

#if IS_WPF
[ValueConversion(typeof(TimeSpan), typeof(string))]
#endif
public sealed class NiceTimeSpanConverter : IValueConverter
{
    public string DatesFormat { get; set; } = SR.DatesTimeSpanFormat;
    public string HoursFormat { get; set; } = SR.HoursTimeSpanFormat;
    public string MinutesFormat { get; set; } = SR.MinutesTimeSpanFormat;
    public string SecondsFormat { get; set; } = SR.SecondsTimeSpanFormat;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var ts = value is TimeSpan v ? v : value is string s && TimeSpan.TryParse(s, out var v2) ? v2 : (TimeSpan?)null;
        if (ts != null)
        {
            var c = ts.Value;
            if (c >= TimeSpan.FromDays(1))
            {
                return c.ToString(DatesFormat, culture);
            }
            if (c >= TimeSpan.FromHours(1))
            {
                return c.ToString(HoursFormat, culture);
            }
            if (c >= TimeSpan.FromMinutes(1))
            {
                return c.ToString(MinutesFormat, culture);
            }
            return c.ToString(SecondsFormat, culture);
        }
        return null;
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
