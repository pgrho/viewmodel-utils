namespace Shipwreck.ViewModelUtils.Components;

public abstract class FormGroupBase : BindableComponentBase
{
    [CascadingParameter]
    public FormGroupTheme Theme { get; set; }

    #region FormGroupId

    private static ulong _FormGroupNextId = 0;
    private string _FormGroupId;

    [Parameter]
    public string FormGroupId
    {
        get => _FormGroupId ??= ("form--group--" + Interlocked.Increment(ref _FormGroupNextId));
        set => SetProperty(ref _FormGroupId, value);
    }

    #endregion FormGroupId

    [Parameter]
    public string AdditionalClass { get; set; }

    [Parameter]
    public string AdditionalLabelClass { get; set; }

    [Parameter]
    public string AdditionalInputClass { get; set; }

    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public string Description { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    #region IsEnabled

    [Parameter]
    public bool IsEnabled { get; set; } = true;

    protected virtual bool GetIsDisabled()
        => !IsEnabled;

    #endregion IsEnabled

    #region IsRequired

    [Parameter]
    public bool IsRequired { get; set; }

    protected virtual bool GetIsRequired() => IsRequired;

    #endregion IsRequired

    #region Placeholder

    [Parameter]
    public string Placeholder { get; set; }

    protected virtual string GetPlaceholder() => Placeholder;

    #endregion Placeholder

    [Parameter]
    public ICommand OnFocusCommand { get; set; }

    [Parameter]
    public object OnFocusCommandParameter { get; set; }

    [Parameter]
    public ICommand EnterCommand { get; set; }

    [Parameter]
    public object EnterCommandParameter { get; set; }

    protected override bool ImplicitRender => false;

    public abstract ValueTask FocusAsync(bool selectAll = false);
}
