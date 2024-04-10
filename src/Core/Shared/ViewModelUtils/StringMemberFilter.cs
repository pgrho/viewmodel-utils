namespace Shipwreck.ViewModelUtils;

public sealed class StringMemberFilter<T> : IMemberFilter<T>
{
    private readonly Func<T, string?> _Selector;

    private readonly Action<StringMemberFilter<T>> _OnChanged;

    private const string DEFAULT_DESCRIPTION = null;

    public StringMemberFilter(Func<T, string?> selector, Action<StringMemberFilter<T>> onChanged, string name = null, string description = null)
    {
        _Selector = selector;
        _OnChanged = onChanged;
        Name = name;
        Description = description ?? DEFAULT_DESCRIPTION;
    }

    public string Name { get; }
    public string Description { get; }

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
