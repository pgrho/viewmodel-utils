namespace Shipwreck.ViewModelUtils.Components;

internal abstract class PropertyTableColumnBuilder<T> : PropertyTableColumnBuilder
{
    protected abstract PropertyTableColumn<T> CreateTableColumnCore();

    protected override TableColumn CreateTableColumn()
    {
        var c = CreateTableColumnCore();
        c.GetValueDelegate = GetGetValueDelegate<T>();
        c.SetValueDelegate = GetSetValueDelegate<T>();
        c.OptionsDelegate = GetOptionsDelegate<T>();
        c.InputAttributes = GetAdditionalAttributes();
        return c;
    }
}
