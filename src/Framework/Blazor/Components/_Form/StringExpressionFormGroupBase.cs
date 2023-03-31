namespace Shipwreck.ViewModelUtils.Components;

public abstract class StringExpressionFormGroupBase : ExpressionBoundFormGroup<string>
{
    #region Placeholder

    [Parameter]
    public string Placeholder { get; set; }

    protected virtual string GetPlaceholder()
        => Placeholder;

    #endregion Placeholder

    #region IsReadOnly

    [Parameter]
    public bool? IsReadOnly { get; set; }

    protected virtual bool GetIsReadOnly()
        => IsReadOnly ?? (Validator?.IsEditable == false);

    protected override bool GetIsDisabled()
        => !IsEnabled;

    #endregion IsReadOnly

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

    protected override void OnExpressionChanged()
    {
        base.OnExpressionChanged();
        _ExpressionMaxLength = 0;
    }
}
