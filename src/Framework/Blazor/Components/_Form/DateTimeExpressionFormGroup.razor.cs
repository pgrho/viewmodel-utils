namespace Shipwreck.ViewModelUtils.Components;

public partial class DateTimeExpressionFormGroup : ExpressionBoundFormGroup<DateTime?>
{
    [Parameter]
    public string Placeholder { get; set; }

    #region IsReadOnly

    [Parameter]
    public bool IsReadOnly { get; set; }

    protected virtual bool GetIsReadOnly()
        => IsReadOnly || (Validator?.IsEditable == false);

    protected override bool GetIsDisabled()
        => !IsEnabled;

    #endregion IsReadOnly

    [Parameter]
    public DateTimePickerMode Mode { get; set; } = DateTimePickerMode.Date;
}
