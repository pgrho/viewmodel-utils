namespace Shipwreck.ViewModelUtils.Components;

public partial class DateTimeExpressionFormGroup : ExpressionBoundFormGroup<DateTime?>
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

    #region Mode

    private DateTimePickerMode _Mode = DateTimePickerMode.Date;

    [Parameter]
    public DateTimePickerMode Mode
    {
        get => _Mode;
        set => SetProperty(ref _Mode, value);
    }

    #endregion Mode
}
