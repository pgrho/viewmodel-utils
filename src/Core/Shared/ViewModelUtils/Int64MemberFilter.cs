namespace Shipwreck.ViewModelUtils;

public sealed partial class Int64MemberFilter<T> : IMemberFilter<T>
{
    private readonly Func<T, long?> _Selector;

    private readonly Action<Int64MemberFilter<T>> _OnChanged;

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
    private readonly static string DEFAULT_DESCRIPTION = $@"整数値を検索します。
{EQ_OPERATOR}: 一致
{NE_OPERATOR}: 不一致
{LT_OPERATOR}: 未満
{LTE_OPERATOR}: 以下
{GT_OPERATOR}: 超過
{GTE_OPERATOR}: 以上
{BETWEEN_OPERATOR}: 範囲";

    public Int64MemberFilter(Func<T, long?> selector, Action<Int64MemberFilter<T>> onChanged, string name = null, string description = null)
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
    private string _Operator;
    private long? _Operand;
    private long? _Operand2;

    private const string _BETWEEN_PATTERN = "^([0-9]+)\\.\\.([0-9]+)$";
#if NET7_0_OR_GREATER
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

                _Operator = null;
                _Operand = null;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (BetweenPattern().Match(value) is var bm
                        && bm.Success
                        && long.TryParse(bm.Groups[1].Value, out var lv1)
                        && long.TryParse(bm.Groups[2].Value, out var lv2))
                    {
                        _Operator = BETWEEN_OPERATOR;
                        _Operand = lv1;
                        _Operand2 = lv2;
                    }
                    else
                    {
                        foreach (var op in _Operators)
                        {
                            if (value.StartsWith(op))
                            {
                                _Operator = op;
                                if (long.TryParse(value.Substring(op.Length), out var lv))
                                {
                                    _Operand = lv;
                                }
                                else
                                {
                                    _Operand = null;
                                    _Operator = string.Empty;
                                }
                                break;
                            }
                        }
                        if (_Operator == null)
                        {
                            if (long.TryParse(value, out var lv))
                            {
                                _Operator = EQ_OPERATOR;
                                _Operand = lv;
                            }
                            else
                            {
                                _Operand = null;
                                _Operator = string.Empty;
                            }
                        }
                    }
                }
                _OnChanged(this);
            }
        }
    }

    #endregion Filter

    public bool IsMatch(T item)
    {
        if (_Operator == null)
        {
            return true;
        }
        if (_Operand == null)
        {
            return false;
        }
        var v = _Selector(item);

        switch (_Operator)
        {
            default:
                return false;

            case EQ_OPERATOR:
                return v == _Operand;

            case NE_OPERATOR:
                return v != _Operand;

            case LTE_OPERATOR:
                return v <= _Operand;

            case GTE_OPERATOR:
                return v >= _Operand;

            case LT_OPERATOR:
                return v < _Operand;

            case GT_OPERATOR:
                return v > _Operand;

            case BETWEEN_OPERATOR:
                return _Operand <= v && v <= _Operand2;
        }
    }
}
