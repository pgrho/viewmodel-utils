namespace Shipwreck.ViewModelUtils.Components;

public sealed class SelectionTableColumnBuilder : TableColumnBuilder
{
    public override TableColumn Build()
        => new SelectionTableColumn(GetIsReadOnlyDelegate());
}
