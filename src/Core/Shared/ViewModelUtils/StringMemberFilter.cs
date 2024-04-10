namespace Shipwreck.ViewModelUtils;

public sealed class StringMemberFilter<T> : IMemberFilter<T>
{
    private readonly Func<T, string?> _Selector;

    private readonly Action<StringMemberFilter<T>> _OnChanged;

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

                _Operands = null;
                if (string.IsNullOrWhiteSpace(value))
                {
                    _Operator = null;
                }
                else if (value.StartsWith(LIST_IN_OPERATOR))
                {
                    _Operator = LIST_IN_OPERATOR;
                    _Operand = value.Substring(LIST_IN_OPERATOR.Length);
                    _Operands = _Operand.Split(',');
                }
                else
                {
                    _Operator = string.Empty;
                    _Operand = value;
                    foreach (var op in _Operators)
                    {
                        if (value.StartsWith(op))
                        {
                            _Operator = op;
                            _Operand = value.Substring(op.Length);
                            break;
                        }
                    }
                }
                _OnChanged(this);
            }
        }
    }

    private string _Operator;
    private string _Operand;
    private string[] _Operands;

    #endregion Filter

    public bool IsMatch(T item)
    {
        if (_Operator == null)
        {
            return true;
        }
        var v = _Selector(item);

        static int compare(string v, string op)
            => CultureInfo.InvariantCulture.CompareInfo.Compare(
                v ?? string.Empty,
                op ?? string.Empty,
                CompareOptions.IgnoreCase
                | CompareOptions.IgnoreKanaType
                | CompareOptions.IgnoreNonSpace);

        static int indexOf(string v, string op)
            => CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                v ?? string.Empty,
                op ?? string.Empty,
                CompareOptions.IgnoreCase
                | CompareOptions.IgnoreKanaType
                | CompareOptions.IgnoreNonSpace);

        switch (_Operator)
        {
            case EQ_OPERATOR:
                return compare(v, _Operand) == 0;

            case NE_OPERATOR:
                return compare(v, _Operand) != 0;

            case LTE_OPERATOR:
                return compare(v, _Operand) <= 0;

            case GTE_OPERATOR:
                return compare(v, _Operand) >= 0;

            case LT_OPERATOR:
                return compare(v, _Operand) < 0;

            case GT_OPERATOR:
                return compare(v, _Operand) > 0;

            case STR_IN_OPERATOR:
            default:
                return indexOf(v, _Operand) >= 0;

            case STARTS_WITH_OPERATOR:
                return indexOf(v, _Operand) == 0;

            case ENDS_WITH_OPERATOR:
                return indexOf(v, _Operand) == v?.Length - _Operand?.Length;

            case LIST_IN_OPERATOR:
                foreach (var op in _Operands ?? Enumerable.Empty<string>())
                {
                    if (compare(v, op) == 0)
                    {
                        return true;
                    }
                }
                return false;
        }
    }
}
