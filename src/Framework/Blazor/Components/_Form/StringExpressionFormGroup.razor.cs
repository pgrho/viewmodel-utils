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

    private string _Pattern;
    private string _ExpressionPattern;

    [Parameter]
    public string Pattern
    {
        get => _Pattern;
        set => SetProperty(ref _Pattern, value);
    }

    protected virtual string GetPattern()
    {
        if (_Pattern != null)
        {
            return _Pattern;
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
