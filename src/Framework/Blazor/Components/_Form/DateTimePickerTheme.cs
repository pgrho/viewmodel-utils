
namespace Shipwreck.ViewModelUtils.Components;

public sealed class DateTimePickerTheme
{
    public DateTimePickerTheme()
        : this(DateTimeFormatInfo.CurrentInfo)
    { }

    public DateTimePickerTheme(DateTimeFormatInfo dateTimeFormat)
    {
        DayOfWeeks = new[]
        {
            DayOfWeek.Sunday,
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday,
        }.Select(e => dateTimeFormat?.GetShortestDayName(e) ?? e.ToString("G").Substring(0, 2)).ToArray();
    }
    public static DateTimePickerTheme Default { get; } = new DateTimePickerTheme(CultureInfo.InvariantCulture.DateTimeFormat);

    public DateTimePickerButtonTheme Button { get; set; } = new DateTimePickerButtonTheme()
    {
        Class = "btn btn-sm btn-block border-0 btn-outline-secondary"
    };
    public DateTimePickerButtonTheme Selected { get; set; } = new DateTimePickerButtonTheme()
    {
        Class = "btn btn-sm btn-block border-0 btn-primary"
    };
    public DateTimePickerButtonTheme OtherMonth { get; set; } = new DateTimePickerButtonTheme()
    {
        Class = "btn btn-sm btn-block border-0 btn-outline-secondary",
        Style = "opacity: 0.75"
    };
    public DateTimePickerButtonTheme Sunday { get; set; } = new DateTimePickerButtonTheme()
    {
        Class = "btn btn-sm btn-block border-0 btn-outline-danger"
    };
    public DateTimePickerButtonTheme Saturday { get; set; } = new DateTimePickerButtonTheme()
    {
        Class = "btn btn-sm btn-block border-0 btn-outline-info"
    };

    public IList<string> DayOfWeeks { get; set; }
}
