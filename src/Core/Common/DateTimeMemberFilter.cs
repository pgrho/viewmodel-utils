namespace Shipwreck.ViewModelUtils;

public sealed partial class DateTimeMemberFilter<T> : IMemberFilter<T>
{
    private readonly Func<T, DateTime?> _Selector;

    private readonly Action<DateTimeMemberFilter<T>>? _OnChanged;

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

    internal const string BETWEEN_OPERATOR = "..";

    public static string EqualOperator => EQ_OPERATOR;
    public static string NotEqualOperator => NE_OPERATOR;
    public static string LessThanOperator => LT_OPERATOR;
    public static string LessThanOrEqualOperator => LTE_OPERATOR;
    public static string GreaterThanOperator => GT_OPERATOR;
    public static string GreaterThanOrEqualOperator => GTE_OPERATOR;
    public static string BetweenOperator => BETWEEN_OPERATOR;

    private readonly static string DEFAULT_DESCRIPTION = $@"日付を検索します。
{EQ_OPERATOR}: 一致
{NE_OPERATOR}: 不一致
{LT_OPERATOR}: 未満
{LTE_OPERATOR}: 以下
{GT_OPERATOR}: 超過
{GTE_OPERATOR}: 以上
{BETWEEN_OPERATOR}: 範囲";

    public DateTimeMemberFilter(Func<T, DateTime?> selector, Action<DateTimeMemberFilter<T>>? onChanged = null, string? name = null, string? description = null)
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

    public bool IsInclude { get; private set; }
    public DateTime? ParsedLowerBound { get; private set; }
    public DateTime? ParsedUpperBound { get; private set; }

    private const string DATE1_PATTERN = "(?<y>[0-9]{4})(?:(?<sep>/|-|)(?<m>[0-9]{1,2})(?:\\k<sep>(?<d>[0-9]{1,2}))?)?";
    private const string DATE2_PATTERN = "(?<y2>[0-9]{4})(?:\\k<sep>(?<m2>[0-9]{1,2})(?:\\k<sep>(?<d2>[0-9]{1,2}))?)?";

    private const string _SINGLE_PATTERN = $"^(?<op>|!?=|[<>]=?)" + DATE1_PATTERN + "$";
    private const string _BETWEEN_PATTERN = "^" + DATE1_PATTERN + "\\.\\." + DATE2_PATTERN + "$";
#if NET7_0_OR_GREATER
    [GeneratedRegex(_SINGLE_PATTERN)]
    private static partial Regex SinglePattern();
    [GeneratedRegex(_BETWEEN_PATTERN)]
    private static partial Regex BetweenPattern();
#else
    private static readonly Regex _SinglePattern = new(_SINGLE_PATTERN);
    private static Regex SinglePattern() => _SinglePattern;
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
                if (!string.IsNullOrWhiteSpace(value))
                {
                    static bool parse(bool hasSeparator, string y, string m, string d, out DateTime? lower, out DateTime? upper)
                    {
                        lower = upper = null;
                        if (!int.TryParse(y, out var year) || year < 1 || 9999 < year)
                        {
                            return false;
                        }

                        if (string.IsNullOrEmpty(m))
                        {
                            if (year == 9999)
                            {
                                return false;
                            }
                            lower = new DateTime(year, 1, 1);
                            upper = new DateTime(year + 1, 1, 1);
                            return true;
                        }
                        else
                        {
                            if ((!hasSeparator && m.Length != 2)
                                || !int.TryParse(m, out var month) || month < 1 || 12 < month)
                            {
                                return false;
                            }

                            if (string.IsNullOrEmpty(d))
                            {
                                if (year == 9999 && month == 12)
                                {
                                    return false;
                                }
                                lower = new DateTime(year, month, 1);
                                upper = new DateTime(year, month, 1).AddMonths(1);
                                return true;
                            }

                            if ((!hasSeparator && d.Length != 2)
                                || !int.TryParse(d, out var day) || day < 1 || DateTime.DaysInMonth(year, month) < day)
                            {
                                return false;
                            }
                            lower = new DateTime(year, month, day);
                            upper = new DateTime(year, month, day).AddDays(1);
                            return true;
                        }
                    }

                    if (SinglePattern().Match(value) is var sm && sm.Success)
                    {
                        IsInclude = true;
                        var hasSeparator = sm.Groups["sep"]?.Length > 0;
                        if (parse(hasSeparator, sm.Groups["y"].Value, sm.Groups["m"].Value, sm.Groups["d"].Value, out var lb, out var ub))
                        {
                            var op = sm.Groups["op"].Value;
                            switch (op)
                            {
                                case EQ_OPERATOR:
                                case NE_OPERATOR:
                                default:
                                    IsInclude = op != NE_OPERATOR;
                                    ParsedLowerBound = lb;
                                    ParsedUpperBound = ub;
                                    break;

                                case LT_OPERATOR:
                                    IsInclude = true;
                                    ParsedLowerBound = DateTime.MinValue;
                                    ParsedUpperBound = lb;
                                    break;

                                case LTE_OPERATOR:
                                    IsInclude = true;
                                    ParsedLowerBound = DateTime.MinValue;
                                    ParsedUpperBound = ub;
                                    break;

                                case GT_OPERATOR:
                                    IsInclude = true;
                                    ParsedLowerBound = ub;
                                    ParsedUpperBound = DateTime.MaxValue;
                                    break;

                                case GTE_OPERATOR:
                                    IsInclude = true;
                                    ParsedLowerBound = lb;
                                    ParsedUpperBound = DateTime.MaxValue;
                                    break;
                            }
                        }
                        else
                        {
                            ParsedLowerBound = null;
                            ParsedUpperBound = null;
                        }
                    }
                    else if (BetweenPattern().Match(value) is var bm && bm.Success)
                    {
                        IsInclude = true;
                        var hasSeparator = bm.Groups["sep"]?.Length > 0;

                        if (parse(hasSeparator, bm.Groups["y"].Value, bm.Groups["m"].Value, bm.Groups["d"].Value, out var lb, out _))
                        {
                            parse(hasSeparator, bm.Groups["y2"].Value, bm.Groups["m2"].Value, bm.Groups["d2"].Value, out _, out var ub);
                            ParsedUpperBound = ub;
                        }
                        ParsedLowerBound = lb;
                    }
                    else
                    {
                        ParsedLowerBound = null;
                        ParsedUpperBound = null;
                    }
                }
                _OnChanged?.Invoke(this);
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
        if (ParsedLowerBound == null || ParsedUpperBound == null)
        {
            return false;
        }
        var v = _Selector(item);
        return (ParsedLowerBound <= v && v < ParsedUpperBound) == IsInclude;
    }
    bool IMemberFilter.IsMatch(object obj) => obj is T item && IsMatch(item);
}
