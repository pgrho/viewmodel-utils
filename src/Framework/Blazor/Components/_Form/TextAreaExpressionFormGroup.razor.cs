namespace Shipwreck.ViewModelUtils.Components;

public partial class TextAreaExpressionFormGroup : StringExpressionFormGroupBase
{
    #region Rows

    private int? _Rows;

    [Parameter]
    public int? Rows
    {
        get => _Rows;
        set => SetProperty(ref _Rows, value);
    }

    #endregion Rows

    #region Columns

    private int? _Columns;

    [Parameter]
    public int? Columns
    {
        get => _Columns;
        set => SetProperty(ref _Columns, value);
    }

    #endregion Columns

    public override ValueTask FocusAsync(bool selectAll = false)
        => JS.FocusAsync(_Input, selectAll: selectAll);
}
