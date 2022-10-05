namespace Shipwreck.ViewModelUtils.Components;

public sealed class ShortDateTimeSpan : DateTimeSpan<DateTime>
{
    public ShortDateTimeSpan()
    {
        DisplayFormat = SR.ShortDateTimeWithDayFormat;
        ToolTipFormat = SR.LongDateTimeFormat;
    }
}
