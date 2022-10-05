namespace Shipwreck.ViewModelUtils.Components;

public sealed class MonthDateSpan : DateTimeSpan<DateTime>
{
    public MonthDateSpan()
    {
        DisplayFormat = SR.MonthDateWithDayFormat;
        ToolTipFormat = SR.LongDateFormat;
    }
}
