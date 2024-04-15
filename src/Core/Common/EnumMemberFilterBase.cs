using Shipwreck.ReflectionUtils;

namespace Shipwreck.ViewModelUtils;

public abstract partial class EnumMemberFilterBase<T, TValue> : IMemberFilter<T>, IEnumMemberFilter
    where TValue : struct
{
    private readonly Func<T, TValue?> _Selector;

    public EnumMemberFilterBase(
        Func<T, TValue?> selector,
        string name = null, string description = null)
    {
        _Selector = selector;
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    protected abstract bool TryParse(string text, out TValue? value);

    private const string _SPLITTER_PATTERN = ",";
#if NET7_0_OR_GREATER
    [GeneratedRegex(_SPLITTER_PATTERN, RegexOptions.IgnoreCase)]
    private static partial Regex SplitterPattern();
#else
    private static readonly Regex _SplitterPattern = new(_SPLITTER_PATTERN, RegexOptions.IgnoreCase);

    private static Regex SplitterPattern() => _SplitterPattern;

#endif

    #region Filter

    private string _Filter = string.Empty;
    private TValue?[]? _Array;

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
                    _Array = null;
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

                    _Array = vs.Count == comps.Length ? vs.Distinct().ToArray() : [];
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
        var a = _Array;
        if (a == null)
        {
            return true;
        }

        var v = _Selector(item);

        return Array.IndexOf(a, v) >= 0;
    }

    bool IMemberFilter.IsMatch(object obj) => obj is T item && IsMatch(item);

    IEnumerable<(string value, string name, bool isSelected)> IEnumMemberFilter.EnumerateOptions()
        => EnumerateValues().Select(v => (GetValue(v), GetDisplayName(v), _Array == null || Array.IndexOf(_Array, v) >= 0));
}
