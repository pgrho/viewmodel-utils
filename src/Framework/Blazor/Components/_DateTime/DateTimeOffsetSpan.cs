namespace Shipwreck.ViewModelUtils.Components;

public sealed class DateTimeOffsetSpan : DateTimeSpan<DateTimeOffset>
{
    public DateTimeOffsetSpan()
    {
        DisplayFormat = SR.ShortDateWithDayFormat;
        ToolTipFormat = SR.LongDateTimeFormat;
        AdditionalFormat = SR.TimeFormat;
    }
}
