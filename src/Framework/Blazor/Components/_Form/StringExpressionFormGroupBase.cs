namespace Shipwreck.ViewModelUtils.Components;

public abstract class StringExpressionFormGroupBase : ExpressionBoundFormGroup<string>
{
    #region Placeholder

    private string _Placeholder;

    [Parameter]
    public string Placeholder
    {
        get => _Placeholder;
        set => SetProperty(ref _Placeholder, value);
    }

    protected virtual string GetPlaceholder()
        => Placeholder;

    #endregion Placeholder

    #region IsReadOnly

    private bool? _IsReadOnly;

    [Parameter]
    public bool? IsReadOnly
    {
        get => _IsReadOnly;
        set => SetProperty(ref _IsReadOnly, value);
    }

    protected virtual bool GetIsReadOnly()
        => _IsReadOnly ?? (Validator?.IsEditable == false);

    protected override bool GetIsDisabled()
        => !IsEnabled;

    #endregion IsReadOnly

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

    protected override void OnExpressionChanged()
    {
        base.OnExpressionChanged();
        _ExpressionMaxLength = 0;
    }
}
