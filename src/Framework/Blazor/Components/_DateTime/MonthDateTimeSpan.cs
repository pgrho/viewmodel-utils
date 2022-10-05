namespace Shipwreck.ViewModelUtils.Components;

public sealed class MonthDateTimeSpan : DateTimeSpan<DateTime>
{
    public MonthDateTimeSpan()
    {
        DisplayFormat = SR.MonthDateTimeWithDayFormat;
        ToolTipFormat = SR.LongDateTimeFormat;
    }
}
