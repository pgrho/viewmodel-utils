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

    [CascadingParameter]
    public FormGroupTheme Theme { get; set; }

    #region FormGroupId

    private static ulong _FormGroupNextId = 0;

    [Parameter]
    public string FormGroupId { get; set; } = ("form--group--" + Interlocked.Increment(ref _FormGroupNextId));

    #endregion FormGroupId

    #region Title

    private string _ExpressionTitle;

    [Parameter]
    public string Title { get; set; }

    protected virtual string GetTitle()
    {
        if (Title != null)
        {
            return Title;
        }
        else if (Member != null)
        {
            return _ExpressionTitle ??= GetDisplayNameFromExpression();
        }
        return null;
    }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    #endregion Title

    [Parameter]
    public string ErrorMessageClass { get; set; }

    protected virtual string GetErrorMessageClass() => ErrorMessageClass ?? (Theme ?? FormGroupTheme.Default)?.ErrorMessageClass;

    [Parameter]
    public string AdditionalClass { get; set; }

    [Parameter]
    public string AdditionalLabelClass { get; set; }

    [Parameter]
    public string AdditionalInputClass { get; set; }
}
