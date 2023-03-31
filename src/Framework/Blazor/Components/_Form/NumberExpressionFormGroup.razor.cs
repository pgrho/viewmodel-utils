namespace Shipwreck.ViewModelUtils.Components;

public partial class NumberExpressionFormGroup<T> : ExpressionBoundFormGroup<T?>
    where T : struct, IEquatable<T>, IComparable<T>, IFormattable, IConvertible
{
    [Parameter]
    public string Placeholder { get; set; }

    #region MaxLength

    private int _ExpressionMaxLength;

    [Parameter]
    public int MaxLength { get; set; }

    protected virtual int? GetMaxLength()
    {
        if (MaxLength > 0)
        {
            return MaxLength;
        }
        else if (Member != null)
        {
            if (_ExpressionMaxLength == 0)
            {
                _ExpressionMaxLength = GetMaxLengthFromExpression() ?? -1;
            }
            return _ExpressionMaxLength > 0 ? _ExpressionMaxLength : (int?)null;
        }
        return null;
    }

    #endregion MaxLength

    #region Max

    private double _ExpressionMax = double.NaN;

    [Parameter]
    public double? Max { get; set; }

    protected virtual double? GetMax()
    {
        if (Max != null)
        {
            return Max;
        }
        else if (Member != null)
        {
            if (double.IsNaN(_ExpressionMax))
            {
                _ExpressionMax = (GetMaxFromExpression() as IConvertible)?.ToDouble(null) ?? double.NaN;
            }
            return double.IsNaN(_ExpressionMax) ? _ExpressionMax : (double?)null;
        }
        return null;
    }

    #endregion Max

    #region Min

    private double _ExpressionMin = double.NaN;

    [Parameter]
    public double? Min { get; set; }

    protected virtual double? GetMin()
    {
        if (Min != null)
        {
            return Min;
        }
        else if (Member != null)
        {
            if (double.IsNaN(_ExpressionMin))
            {
                _ExpressionMin = (GetMinFromExpression() as IConvertible)?.ToDouble(null) ?? double.NaN;
            }
            return double.IsNaN(_ExpressionMin) ? _ExpressionMin : (double?)null;
        }
        return null;
    }

    #endregion Min

    #region Step

    [Parameter]
    public double? Step { get; set; }

    protected virtual double? GetStep() => Step;

    #endregion Step

    #region IsReadOnly

    [Parameter]
    public bool IsReadOnly { get; set; }

    protected virtual bool GetIsReadOnly()
        => IsReadOnly || (Validator?.IsEditable == false);

    protected override bool GetIsDisabled()
        => !IsEnabled;

    #endregion IsReadOnly

    protected override void OnExpressionChanged()
    {
        base.OnExpressionChanged();
        _ExpressionMaxLength = 0;
    }

    public override ValueTask FocusAsync(bool selectAll = false)
        => JS.FocusAsync(_Input, selectAll: selectAll);
}
