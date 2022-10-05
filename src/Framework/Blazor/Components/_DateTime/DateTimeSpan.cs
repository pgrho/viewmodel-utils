namespace Shipwreck.ViewModelUtils.Components;

public partial class DateTimeSpan<T> : BindableComponentBase
    where T : struct, IFormattable
{
    #region Value

    private T? _Value;

    [Parameter]
    public T? Value
    {
        get => _Value;
        set => SetProperty(ref _Value, value);
    }

    #endregion Value

    #region DisplayFormat

    private string _DisplayFormat = SR.LongDateTimeFormat;
    private string _DisplayFormatPrefix;
    private string _DisplayFormatDayOfWeek;
    private string _DisplayFormatSuffix;

    [Parameter]
    public string DisplayFormat
    {
        get => _DisplayFormat;
        set
        {
            if (SetProperty(ref _DisplayFormat, value))
            {
                _DisplayFormatPrefix = null;
            }
        }
    }

    private static readonly Regex _DayOfWeekPattern = new("ddd+");

    private DateTimeSpan<T> InvalidatePrefix()
    {
        if (_DisplayFormatPrefix == null)
        {
            if (string.IsNullOrEmpty(_DisplayFormat))
            {
                _DisplayFormatPrefix = string.Empty;
                _DisplayFormatDayOfWeek = null;
                _DisplayFormatSuffix = null;
            }
            else if (_DayOfWeekPattern.Match(_DisplayFormat) is var m
                && m.Success)
            {
                _DisplayFormatPrefix = _DisplayFormat.Substring(0, m.Index);
                _DisplayFormatDayOfWeek = m.Value;
                _DisplayFormatSuffix = _DisplayFormat.Substring(m.Index + m.Length);
                if (_DisplayFormatSuffix.Length == 1)
                {
                    _DisplayFormatSuffix = "%" + _DisplayFormatSuffix;
                }
            }
            else
            {
                _DisplayFormatPrefix = _DisplayFormat;
                _DisplayFormatDayOfWeek = null;
                _DisplayFormatSuffix = null;
            }
        }
        return this;
    }

    protected string DisplayFormatPrefix
        => InvalidatePrefix()._DisplayFormatPrefix;

    protected string DisplayFormatDayOfWeek
        => InvalidatePrefix()._DisplayFormatDayOfWeek;

    protected string DisplayFormatSuffix
        => InvalidatePrefix()._DisplayFormatSuffix;

    #endregion DisplayFormat

    #region ToolTipFormat

    private string _ToolTipFormat = SR.LongDateTimeFormat;

    [Parameter]
    public string ToolTipFormat
    {
        get => _ToolTipFormat;
        set => SetProperty(ref _ToolTipFormat, value);
    }

    #endregion ToolTipFormat

    #region AdditionalFormat

    private string _AdditionalFormat;

    [Parameter]
    public string AdditionalFormat
    {
        get => _AdditionalFormat;
        set => SetProperty(ref _AdditionalFormat, value);
    }

    #endregion AdditionalFormat
}

public sealed class DateTimeSpan : DateTimeSpan<DateTime>
{
    public DateTimeSpan()
    {
        DisplayFormat = SR.ShortDateWithDayFormat;
        ToolTipFormat = SR.LongDateTimeFormat;
        AdditionalFormat = SR.TimeFormat;
    }
}
