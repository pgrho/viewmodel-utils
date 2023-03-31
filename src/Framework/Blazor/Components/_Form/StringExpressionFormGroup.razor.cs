namespace Shipwreck.ViewModelUtils.Components;

public partial class StringExpressionFormGroup : StringExpressionFormGroupBase
{
    [Parameter]
    public string Type { get; set; }

    public string GetInputType() => Type ?? "text";

    [Parameter]
    public string AutoComplete { get; set; }

    public string GetAutoComplete() => AutoComplete ?? "on";

    #region Pattern

    private string _ExpressionPattern;

    [Parameter]
    public string Pattern { get; set; }

    protected virtual string GetPattern()
    {
        if (Pattern != null)
        {
            return Pattern;
        }
        else if (Member != null)
        {
            return _ExpressionPattern ??= GetRegularExpressionFromExpression();
        }
        return null;
    }

    #endregion Pattern

    protected override void OnExpressionChanged()
    {
        base.OnExpressionChanged();
        _ExpressionPattern = null;
    }

    public override ValueTask FocusAsync(bool selectAll = false)
        => JS.FocusAsync(_Input, selectAll: selectAll);
}
