namespace Shipwreck.ViewModelUtils;

public sealed partial class DateTimeMemberFilter<T> : IMemberFilter<T>
{
    private readonly Func<T, DateTime?> _Selector;

    private readonly Action<DateTimeMemberFilter<T>> _OnChanged;

    public DateTimeMemberFilter(Func<T, DateTime?> selector, Action<DateTimeMemberFilter<T>> onChanged)
    {
        _Selector = selector;
        _OnChanged = onChanged;
    }

    #region Filter

    private string _Filter = string.Empty;

    private DateTime? _LowerBound;
    private DateTime? _UpperBound;

    public string? Filter
    {
        get => _Filter;
        set
        {
            value ??= string.Empty;
            if (_Filter != value)
            {
                _Filter = value;

                if (string.IsNullOrWhiteSpace(value))
                {
                    _LowerBound = null;
                    _UpperBound = null;
                }
                else if (int.TryParse(value, out var iv) && 1 <= iv && iv < 9999)
                {
                    _LowerBound = new DateTime(iv, 1, 1);
                    _UpperBound = _LowerBound?.AddYears(1);
                }
                else if (DateTime.TryParseExact(
                    value,
                    new[] { "yyyy-MM", "yyyy/MM", "yy-MM", "yy/MM", "yyyyMM", "yyyy-M", "yyyy/M", "yy-M", "yy/M" },
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out var ym))
                {
                    _LowerBound = ym;
                    _UpperBound = ym.AddMonths(1);
                }
                else if (DateTime.TryParse(value, out var dt))
                {
                    _LowerBound = dt.Date;
                    _UpperBound = _LowerBound?.AddDays(1);
                }
                _OnChanged(this);
            }
        }
    }

    #endregion Filter

    public bool IsMatch(T item)
    {
        if (string.IsNullOrEmpty(Filter))
        {
            return true;
        }
        if (_LowerBound == null)
        {
            return false;
        }
        var v = _Selector(item);
        return _LowerBound <= v && v < _UpperBound;
    }
}
