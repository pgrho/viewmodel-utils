namespace Shipwreck.ViewModelUtils.Components;

public partial class NumberExpressionFormGroup<T> : ExpressionBoundFormGroup<T?>
    where T : struct, IEquatable<T>, IComparable<T>, IFormattable, IConvertible
{
    #region Placeholder

    private string _Placeholder;

    [Parameter]
    public string Placeholder
    {
        get => _Placeholder;
        set => SetProperty(ref _Placeholder, value);
    }

    #endregion Placeholder

    #region MaxLength

    private int _MaxLength;
    private int _ExpressionMaxLength;

    [Parameter]
    public int MaxLength
    {
        get => _MaxLength;
        set => SetProperty(ref _MaxLength, value);
    }

    protected virtual int? GetMaxLength()
    {
        if (_MaxLength > 0)
        {
            return _MaxLength;
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

    private double? _Max;
    private double _ExpressionMax = double.NaN;

    [Parameter]
    public double? Max
    {
        get => _Max;
        set => SetProperty(ref _Max, value);
    }

    protected virtual double? GetMax()
    {
        if (_Max != null)
        {
            return _Max;
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

    private double? _Min;
    private double _ExpressionMin = double.NaN;

    [Parameter]
    public double? Min
    {
        get => _Min;
        set => SetProperty(ref _Min, value);
    }

    protected virtual double? GetMin()
    {
        if (_Min != null)
        {
            return _Min;
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

    private double? _Step;

    [Parameter]
    public double? Step
    {
        get => _Step;
        set => SetProperty(ref _Step, value);
    }

    protected virtual double? GetStep() => _Step;

    #endregion Step

    #region IsReadOnly

    private bool _IsReadOnly;

    [Parameter]
    public bool IsReadOnly
    {
        get => _IsReadOnly;
        set => SetProperty(ref _IsReadOnly, value);
    }

    protected virtual bool GetIsReadOnly()
        => _IsReadOnly || (Validator?.IsEditable == false);

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
