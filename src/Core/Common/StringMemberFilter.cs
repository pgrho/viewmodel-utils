namespace Shipwreck.ViewModelUtils;

public sealed class StringMemberFilter<T> : IMemberFilter<T>
{
    private readonly Func<T, string?> _Selector;

    private readonly Action<StringMemberFilter<T>>? _OnChanged;

    private static readonly string[] _Operators =
        {
            EQ_OPERATOR,
            NE_OPERATOR,

            LTE_OPERATOR,
            GTE_OPERATOR,
            LT_OPERATOR,
            GT_OPERATOR,

            STR_IN_OPERATOR,
            STARTS_WITH_OPERATOR,
            ENDS_WITH_OPERATOR,

            LIST_IN_OPERATOR,
        };

    internal const string EQ_OPERATOR = "=";
    internal const string NE_OPERATOR = "!=";

    internal const string LT_OPERATOR = "<";
    internal const string LTE_OPERATOR = "<=";
    internal const string GT_OPERATOR = ">";
    internal const string GTE_OPERATOR = ">=";

    internal const string STR_IN_OPERATOR = "*=";
    internal const string STARTS_WITH_OPERATOR = "^=";
    internal const string ENDS_WITH_OPERATOR = "$=";

    internal const string LIST_IN_OPERATOR = "|=";

    public static string EqualOperator => EQ_OPERATOR;
    public static string NotEqualOperator => NE_OPERATOR;
    public static string LessThanOperator => LT_OPERATOR;
    public static string LessThanOrEqualOperator => LTE_OPERATOR;
    public static string GreaterThanOperator => GT_OPERATOR;
    public static string GreaterThanOrEqualOperator => GTE_OPERATOR;
    public static string ContainsOperator => STR_IN_OPERATOR;
    public static string StartsWithOperator => STARTS_WITH_OPERATOR;
    public static string EndsWithOperator => ENDS_WITH_OPERATOR;
    public static string ListInOperator => LIST_IN_OPERATOR;

    private readonly static string DEFAULT_DESCRIPTION = $@"文字列を検索します。
{EQ_OPERATOR}: 一致
{NE_OPERATOR}: 不一致
{LT_OPERATOR}: 未満
{LTE_OPERATOR}: 以下
{GT_OPERATOR}: 超過
{GTE_OPERATOR}: 以上
{STR_IN_OPERATOR}: 含む
{STARTS_WITH_OPERATOR}: 前方一致
{ENDS_WITH_OPERATOR}: 後方一致
{LIST_IN_OPERATOR}: いずれかを含む";

    public StringMemberFilter(Func<T, string?> selector, Action<StringMemberFilter<T>>? onChanged = null, string? name = null, string? description = null)
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

    public string? Filter
    {
        get => _Filter;
        set
        {
            value ??= string.Empty;
            if (_Filter != value)
            {
                _Filter = value;

                ParsedOperands = null;
                if (string.IsNullOrWhiteSpace(value))
                {
                    ParsedOperator = null;
                }
                else if (value.StartsWith(LIST_IN_OPERATOR))
                {
                    ParsedOperator = LIST_IN_OPERATOR;
                    ParsedOperand = value.Substring(LIST_IN_OPERATOR.Length);
                    ParsedOperands = new(ParsedOperand.Split(','));
                }
                else
                {
                    ParsedOperator = string.Empty;
                    ParsedOperand = value;
                    foreach (var op in _Operators)
                    {
                        if (value.StartsWith(op))
                        {
                            ParsedOperator = op;
                            ParsedOperand = value.Substring(op.Length);
                            break;
                        }
                    }
                }
                _OnChanged?.Invoke(this);
            }
        }
    }

    public string? ParsedOperator { get; private set; }
    public string? ParsedOperand { get; private set; }
    public ReadOnlyCollection<string>? ParsedOperands { get; private set; }

    #endregion Filter

    public bool IsMatch(T item)
    {
        if (ParsedOperator == null)
        {
            return true;
        }
        var v = _Selector(item);

        static int compare(string? v, string? op)
            => CultureInfo.InvariantCulture.CompareInfo.Compare(
                v ?? string.Empty,
                op ?? string.Empty,
                CompareOptions.IgnoreCase
                | CompareOptions.IgnoreKanaType
                | CompareOptions.IgnoreNonSpace);

        static int indexOf(string? v, string? op)
            => CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                v ?? string.Empty,
                op ?? string.Empty,
                CompareOptions.IgnoreCase
                | CompareOptions.IgnoreKanaType
                | CompareOptions.IgnoreNonSpace);

        switch (ParsedOperator)
        {
            case EQ_OPERATOR:
                return compare(v, ParsedOperand) == 0;

            case NE_OPERATOR:
                return compare(v, ParsedOperand) != 0;

            case LTE_OPERATOR:
                return compare(v, ParsedOperand) <= 0;

            case GTE_OPERATOR:
                return compare(v, ParsedOperand) >= 0;

            case LT_OPERATOR:
                return compare(v, ParsedOperand) < 0;

            case GT_OPERATOR:
                return compare(v, ParsedOperand) > 0;

            case STR_IN_OPERATOR:
            default:
                return indexOf(v, ParsedOperand) >= 0;

            case STARTS_WITH_OPERATOR:
                return indexOf(v, ParsedOperand) == 0;

            case ENDS_WITH_OPERATOR:
                return indexOf(v, ParsedOperand) == v?.Length - ParsedOperand?.Length;

            case LIST_IN_OPERATOR:
                foreach (var op in ParsedOperands ?? Enumerable.Empty<string>())
                {
                    if (compare(v, op) == 0)
                    {
                        return true;
                    }
                }
                return false;
        }
    }

    bool IMemberFilter.IsMatch(object obj) => obj is T item && IsMatch(item);
}
