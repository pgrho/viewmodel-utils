namespace Shipwreck.ViewModelUtils;

public sealed partial class Int64MemberFilter<T> : IMemberFilter<T>
{
    private readonly Func<T, long?> _Selector;

    private readonly Action<Int64MemberFilter<T>>? _OnChanged;

    private static readonly string[] _Operators =
        {
            EQ_OPERATOR,
            NE_OPERATOR,

            LTE_OPERATOR,
            GTE_OPERATOR,
            LT_OPERATOR,
            GT_OPERATOR,
        };

    internal const string EQ_OPERATOR = "=";
    internal const string NE_OPERATOR = "!=";

    internal const string LT_OPERATOR = "<";
    internal const string LTE_OPERATOR = "<=";
    internal const string GT_OPERATOR = ">";
    internal const string GTE_OPERATOR = ">=";

    internal const string LIST_IN_OPERATOR = "|=";

    internal const string BETWEEN_OPERATOR = "..";

    public static string EqualOperator => EQ_OPERATOR;
    public static string NotEqualOperator => NE_OPERATOR;
    public static string LessThanOperator => LT_OPERATOR;
    public static string LessThanOrEqualOperator => LTE_OPERATOR;
    public static string GreaterThanOperator => GT_OPERATOR;
    public static string GreaterThanOrEqualOperator => GTE_OPERATOR;
    public static string BetweenOperator => BETWEEN_OPERATOR;
    public static string ListInOperator => LIST_IN_OPERATOR;

    private static readonly string DEFAULT_DESCRIPTION = $@"整数値を検索します。
{EQ_OPERATOR}: 一致
{NE_OPERATOR}: 不一致
{LT_OPERATOR}: 未満
{LTE_OPERATOR}: 以下
{GT_OPERATOR}: 超過
{GTE_OPERATOR}: 以上
{LIST_IN_OPERATOR}: いずれかを含む
{BETWEEN_OPERATOR}: 範囲";

    public Int64MemberFilter(Func<T, long?> selector, Action<Int64MemberFilter<T>>? onChanged = null, string? name = null, string? description = null)
    {
        _Selector = selector;
        _OnChanged = onChanged;
        Name = name;
        Description = description ?? DEFAULT_DESCRIPTION;
    }

    public string? Name { get; }
    public string? Description { get; }

    #region Filter

    private string _Filter = string.Empty;
    public string? ParsedOperator { get; private set; }
    public long? ParsedOperand => ParsedOperands?.ElementAtOrDefault(0);
    public long? ParsedOperand2 => ParsedOperands?.ElementAtOrDefault(1);
    public ReadOnlyCollection<long?>? ParsedOperands { get; private set; }

    private const string _BETWEEN_PATTERN = "^([-+]?[0-9]+)\\.\\.([-+]?[0-9]+)$";
#if NET9_0_OR_GREATER
    [GeneratedRegex(_BETWEEN_PATTERN)]
    private static partial Regex BetweenPattern();
#else
    private static readonly Regex _BetweenPattern = new(_BETWEEN_PATTERN);

    private static Regex BetweenPattern() => _BetweenPattern;

#endif

    public string? Filter
    {
        get => _Filter;
        set
        {
            value ??= string.Empty;
            if (_Filter != value)
            {
                _Filter = value;

                ParsedOperator = null;
                ParsedOperands = null;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (BetweenPattern().Match(value) is var bm
                        && bm.Success
                        && long.TryParse(bm.Groups[1].Value, out var lv1)
                        && long.TryParse(bm.Groups[2].Value, out var lv2))
                    {
                        ParsedOperator = BETWEEN_OPERATOR;
                        ParsedOperands = new(new long?[] { lv1, lv2 });
                    }
                    else if (value.StartsWith(LIST_IN_OPERATOR))
                    {
                        ParsedOperator = LIST_IN_OPERATOR;
                        var comps = value.Substring(2).Split(',');
                        var ary = new long?[comps.Length];
                        for (var i = 0; i < comps.Length; i++)
                        {
                            var c = comps[i];
                            if (!string.IsNullOrEmpty(c))
                            {
                                if (long.TryParse(c, out var v))
                                {
                                    ary[i] = v;
                                }
                                else
                                {
                                    ParsedOperator = string.Empty;
                                    break;
                                }
                            }
                        }

                        if (ParsedOperator.Length > 0)
                        {
                            ParsedOperands = new(ary);
                        }
                    }
                    else
                    {
                        foreach (var op in _Operators)
                        {
                            if (value.StartsWith(op))
                            {
                                ParsedOperator = op;
                                if (long.TryParse(value.Substring(op.Length), out var lv))
                                {
                                    ParsedOperands = new(new long?[] { lv });
                                }
                                else
                                {
                                    ParsedOperator = string.Empty;
                                }
                                break;
                            }
                        }
                        if (ParsedOperator == null)
                        {
                            var comps = value.Split(',');
                            var ary = new long?[comps.Length];
                            for (var i = 0; i < comps.Length; i++)
                            {
                                var c = comps[i];
                                if (!string.IsNullOrEmpty(c))
                                {
                                    if (long.TryParse(c, out var v))
                                    {
                                        ary[i] = v;
                                    }
                                    else
                                    {
                                        ParsedOperator = string.Empty;
                                        break;
                                    }
                                }
                            }

                            if (ParsedOperator == null)
                            {
                                ParsedOperator = ary.Length == 1 ? EQ_OPERATOR : LIST_IN_OPERATOR;
                                ParsedOperands = new(ary);
                            }
                        }
                    }
                }
                _OnChanged?.Invoke(this);
            }
        }
    }

    #endregion Filter

    public bool IsMatch(T item)
    {
        if (ParsedOperator == null)
        {
            return true;
        }
        if (!(ParsedOperands?.Count > 0))
        {
            return false;
        }
        var v = _Selector(item);

        switch (ParsedOperator)
        {
            default:
                return false;

            case EQ_OPERATOR:
                return v == ParsedOperand;

            case NE_OPERATOR:
                return v != ParsedOperand;

            case LTE_OPERATOR:
                return v <= ParsedOperand;

            case GTE_OPERATOR:
                return v >= ParsedOperand;

            case LT_OPERATOR:
                return v < ParsedOperand;

            case GT_OPERATOR:
                return v > ParsedOperand;

            case LIST_IN_OPERATOR:
                return ParsedOperands.Contains(v);

            case BETWEEN_OPERATOR:
                return ParsedOperand <= v && v <= ParsedOperand2;
        }
    }

    bool IMemberFilter.IsMatch(object obj) => obj is T item && IsMatch(item);
}
