namespace Shipwreck.ViewModelUtils.Components;

public abstract class ExpressionBoundFormGroup<T> : ExpressionBoundComponent<T>
{
    [Inject]
    public IJSRuntime JS { get; set; }

    protected override void OnExpressionChanged()
    {
        base.OnExpressionChanged();
        _ExpressionTitle = null;
    }

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

    #region Title

    private string _Title;
    private string _ExpressionTitle;

    [Parameter]
    public string Title
    {
        get => _Title;
        set => SetProperty(ref _Title, value);
    }

    protected virtual string GetTitle()
    {
        if (_Title != null)
        {
            return _Title;
        }
        else if (Member != null)
        {
            return _ExpressionTitle ??= GetDisplayNameFromExpression();
        }
        return null;
    }

    #region ChildContent

    private RenderFragment _ChildContent;

    [Parameter]
    public RenderFragment ChildContent
    {
        get => _ChildContent;
        set => SetProperty(ref _ChildContent, value);
    }

    #endregion ChildContent

    #endregion Title

    #region ErrorMessageClass

    private string _ErrorMessageClass;

    [Parameter]
    public string ErrorMessageClass
    {
        get => _ErrorMessageClass;
        set => SetProperty(ref _ErrorMessageClass, value);
    }

    protected virtual string GetErrorMessageClass() => _ErrorMessageClass ?? (_Theme ?? FormGroupTheme.Default)?.ErrorMessageClass;

    #endregion ErrorMessageClass

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
}
