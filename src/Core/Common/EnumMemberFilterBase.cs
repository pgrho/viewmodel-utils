namespace Shipwreck.ViewModelUtils;

public abstract partial class EnumMemberFilterBase<T, TValue> : IMemberFilter<T>, IEnumMemberFilter
    where TValue : struct
{
    private readonly Func<T, TValue?> _Selector;

    public EnumMemberFilterBase(
        Func<T, TValue?> selector,
        string? name = null, string? description = null)
    {
        _Selector = selector;
        Name = name;
        Description = description;
    }

    public string? Name { get; }
    public string? Description { get; }

    protected abstract bool TryParse(string text, out TValue? value);

    private const string _SPLITTER_PATTERN = ",";
#if NET9_0_OR_GREATER
    [GeneratedRegex(_SPLITTER_PATTERN, RegexOptions.IgnoreCase)]
    private static partial Regex SplitterPattern();
#else
    private static readonly Regex _SplitterPattern = new(_SPLITTER_PATTERN, RegexOptions.IgnoreCase);

    private static Regex SplitterPattern() => _SplitterPattern;

#endif

    #region Filter

    private string _Filter = string.Empty;
    public ReadOnlyCollection<TValue?>? Values { get; private set; }

    public string? Filter
    {
        get => _Filter;
        set
        {
            value ??= string.Empty;
            if (_Filter != value)
            {
                _Filter = value;

                if (string.IsNullOrEmpty(value))
                {
                    Values = null;
                }
                else
                {
                    var comps = SplitterPattern().Split(value);

                    var vs = new List<TValue?>(comps.Length);

                    foreach (var c in comps)
                    {
                        if (TryParse(c, out var v))
                        {
                            vs.Add(v);
                        }
                    }

                    Values = new(vs.Count == comps.Length ? vs.Distinct().ToArray() : []);
                }

                OnChanged();
            }
        }
    }

    protected abstract void OnChanged();

    #endregion Filter

    protected abstract IEnumerable<TValue?> EnumerateValues();

    protected virtual string GetValue(TValue? value)
        => value?.ToString() ?? string.Empty;

    protected virtual string GetDisplayName(TValue? value)
        => value?.ToString() ?? string.Empty;

    public bool IsMatch(T item)
    {
        var a = Values;
        if (a == null)
        {
            return true;
        }

        var v = _Selector(item);

        return a.Contains(v);
    }

    bool IMemberFilter.IsMatch(object obj) => obj is T item && IsMatch(item);

    IEnumerable<FilterOption> IEnumMemberFilter.EnumerateOptions()
        => EnumerateValues()
            .Select(v => new FilterOption(
                GetValue(v),
                GetDisplayName(v),
                Values?.Contains(v) != false));
}
