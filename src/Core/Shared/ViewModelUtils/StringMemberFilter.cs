namespace Shipwreck.ViewModelUtils;

public sealed class StringMemberFilter<T> : IMemberFilter<T>
{
    private readonly Func<T, string?> _Selector;

    private readonly Action<StringMemberFilter<T>> _OnChanged;

    public StringMemberFilter(Func<T, string?> selector, Action<StringMemberFilter<T>> onChanged)
    {
        _Selector = selector;
        _OnChanged = onChanged;
    }

    #region Filter

    private string _Filter = string.Empty;

    public string? Filter
    {
        get => _Filter;
        set
        {
            value ??= string.Empty;
            if (_Filter != value)
            {
                _Filter = value;
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
        var v = _Selector(item);
        return v?.Contains(Filter) == true;
    }
}
