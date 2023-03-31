namespace Shipwreck.ViewModelUtils.Components;

public partial class TextAreaExpressionFormGroup : StringExpressionFormGroupBase
{
    [Parameter]
    public int? Rows { get; set; }

    [Parameter]
    public int? Columns { get; set; }

    public override ValueTask FocusAsync(bool selectAll = false)
        => JS.FocusAsync(_Input, selectAll: selectAll);
}
