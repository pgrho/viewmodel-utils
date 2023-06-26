namespace Shipwreck.ViewModelUtils.Components;

internal sealed class BooleanTableColumnBuilder : PropertyTableColumnBuilder
{
    protected override TableColumn CreateTableColumn()
        => new BooleanTableColumn()
        {
            GetValueDelegate = GetGetValueDelegate<bool>(),
            SetValueDelegate = GetSetValueDelegate<bool>()
        };
}
