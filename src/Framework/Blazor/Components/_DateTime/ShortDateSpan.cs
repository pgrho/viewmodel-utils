namespace Shipwreck.ViewModelUtils.Components;

public sealed class ShortDateSpan : DateTimeSpan<DateTime>
{
    public ShortDateSpan()
    {
        DisplayFormat = SR.ShortDateWithDayFormat;
        ToolTipFormat = SR.LongDateFormat;
    }
}
