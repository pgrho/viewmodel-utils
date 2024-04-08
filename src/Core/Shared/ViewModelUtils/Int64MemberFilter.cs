namespace Shipwreck.ViewModelUtils;

public sealed class Int64MemberFilter<T> : IMemberFilter<T>
{
    private readonly Func<T, long?> _Selector;

    private readonly Action<Int64MemberFilter<T>> _OnChanged;

    public Int64MemberFilter(Func<T, long?> selector, Action<Int64MemberFilter<T>> onChanged)
    {
        _Selector = selector;
        _OnChanged = onChanged;
    }

    #region Filter

    private string _Filter = string.Empty;
    private long? _FilterValue;

    public string? Filter
    {
        get => _Filter;
        set
        {
            value ??= string.Empty;
            if (_Filter != value)
            {
                _Filter = value;
                if (string.IsNullOrWhiteSpace(_Filter) || !long.TryParse(_Filter, out var lv))
                {
                    _FilterValue = null;
                }
                else
                {
                    _FilterValue = lv;
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
        if (_FilterValue == null)
        {
            return false;
        }
        var v = _Selector(item);
        return _FilterValue == v;
    }
}
