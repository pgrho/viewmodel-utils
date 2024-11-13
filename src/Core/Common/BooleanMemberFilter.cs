namespace Shipwreck.ViewModelUtils;

public sealed partial class BooleanMemberFilter<T> : EnumMemberFilterBase<T, bool>
{
    private readonly Action<BooleanMemberFilter<T>>? _OnChanged;

    private readonly static string? DEFAULT_DESCRIPTION = null;

    public BooleanMemberFilter(Func<T, bool?> selector, Action<BooleanMemberFilter<T>>? onChanged = null, string? name = null, string? description = null)
        : base(selector, name: name, description: description ?? DEFAULT_DESCRIPTION)
    {
        _OnChanged = onChanged;
    }

    private const string _TRUE_PATTERN = "^(?:[+-]?1|t(?:rue)?|y(?:es)?)$";
    private const string _FALSE_PATTERN = "^(?:[+-]?0|f(?:alse)?|no?)$";
    private const string _NULL_PATTERN = "^null$";
#if NET9_0_OR_GREATER
    [GeneratedRegex(_TRUE_PATTERN, RegexOptions.IgnoreCase)]
    private static partial Regex TruePattern();
    [GeneratedRegex(_FALSE_PATTERN, RegexOptions.IgnoreCase)]
    private static partial Regex FalsePattern();
    [GeneratedRegex(_NULL_PATTERN, RegexOptions.IgnoreCase)]
    private static partial Regex NullPattern();
#else
    private static readonly Regex _TruePattern = new(_TRUE_PATTERN, RegexOptions.IgnoreCase);
    private static Regex TruePattern() => _TruePattern;
    private static readonly Regex _FalsePattern = new(_FALSE_PATTERN, RegexOptions.IgnoreCase);
    private static Regex FalsePattern() => _FalsePattern;
    private static readonly Regex _NullPattern = new(_NULL_PATTERN, RegexOptions.IgnoreCase);
    private static Regex NullPattern() => _NullPattern;
#endif

    protected override bool TryParse(string text, out bool? value)
    {
        if (string.IsNullOrEmpty(text))
        {
            value = null;
            return true;
        }
        if (TruePattern().IsMatch(text))
        {
            value = true;
            return true;
        }
        if (FalsePattern().IsMatch(text))
        {
            value = false;
            return true;
        }
        if (NullPattern().IsMatch(text))
        {
            value = null;
            return true;
        }

        value = null;
        return false;
    }

    protected override IEnumerable<bool?> EnumerateValues()
        => [null, true, false];

    protected override string GetValue(bool? value)
        => value switch
        {
            true => "t",
            false => "f",
            _ => "null"
        };

    protected override void OnChanged()
        => _OnChanged?.Invoke(this);
}
