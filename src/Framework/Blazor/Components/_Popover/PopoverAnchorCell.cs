namespace Shipwreck.ViewModelUtils.Components;

public abstract class PopoverAnchorCell<T> : BindableComponentBase<T>
    where T : class
{
    #region ChildContent

    private RenderFragment _ChildContent;

    [Parameter]
    public RenderFragment ChildContent
    {
        get => _ChildContent;
        set => SetProperty(ref _ChildContent, value);
    }

    #endregion ChildContent

    #region IsPrimary

    private bool _IsPrimary = true;

    [Parameter]
    public bool IsPrimary
    {
        get => _IsPrimary;
        set => SetProperty(ref _IsPrimary, value);
    }

    #endregion IsPrimary

    [Parameter]
    [Obsolete]
    public bool IsDark
    {
        get => !IsPrimary;
        set => IsPrimary = !value;
    }

    #region Command

    private ICommand _Command;

    [Parameter]
    public ICommand Command
    {
        get => _Command;
        set => SetProperty(ref _Command, value);
    }

    #endregion Command

    #region CommandMode

    private PopoverTargetCommandMode _CommandMode = PopoverTargetCommandMode.Replace;

    [Parameter]
    public PopoverTargetCommandMode CommandMode
    {
        get => _CommandMode;
        set => SetProperty(ref _CommandMode, value);
    }

    #endregion CommandMode

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> AdditionalAttributes
    {
        get => _AdditionalAttributes;
        set => SetProperty(ref _AdditionalAttributes, value);
    }

    private IDictionary<string, object> _AdditionalAttributes;
}
