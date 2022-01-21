namespace Shipwreck.ViewModelUtils.Components;

public abstract class FormGroupBase : BindableComponentBase
{
    #region Theme

    private FormGroupTheme _Theme;

    [CascadingParameter]
    public FormGroupTheme Theme
    {
        get => _Theme;
        set => SetProperty(ref _Theme, value);
    }

    #endregion Theme

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

    #region AdditionalClass

    private string _AdditionalClass;

    [Parameter]
    public string AdditionalClass
    {
        get => _AdditionalClass;
        set => SetProperty(ref _AdditionalClass, value);
    }

    #endregion AdditionalClass

    #region AdditionalLabelClass

    private string _AdditionalLabelClass;

    [Parameter]
    public string AdditionalLabelClass
    {
        get => _AdditionalLabelClass;
        set => SetProperty(ref _AdditionalLabelClass, value);
    }

    #endregion AdditionalLabelClass

    #region AdditionalInputClass

    private string _AdditionalInputClass;

    [Parameter]
    public string AdditionalInputClass
    {
        get => _AdditionalInputClass;
        set => SetProperty(ref _AdditionalInputClass, value);
    }

    #endregion AdditionalInputClass

    #region Title

    private string _Title;

    [Parameter]
    public string Title
    {
        get => _Title;
        set => SetProperty(ref _Title, value);
    }

    #endregion Title

    #region Description

    private string _Description;

    [Parameter]
    public string Description
    {
        get => _Description;
        set => SetProperty(ref _Description, value);
    }

    #endregion Description

    #region ChildContent

    private RenderFragment _ChildContent;

    [Parameter]
    public RenderFragment ChildContent
    {
        get => _ChildContent;
        set => SetProperty(ref _ChildContent, value);
    }

    #endregion ChildContent

    #region IsEnabled

    private bool _IsEnabled = true;

    [Parameter]
    public bool IsEnabled
    {
        get => _IsEnabled;
        set => SetProperty(ref _IsEnabled, value);
    }

    #endregion IsEnabled

    [Parameter]
    public ICommand OnFocusCommand { get; set; }

    [Parameter]
    public object OnFocusCommandParameter { get; set; }

    [Parameter]
    public ICommand EnterCommand { get; set; }

    [Parameter]
    public object EnterCommandParameter { get; set; }
}
