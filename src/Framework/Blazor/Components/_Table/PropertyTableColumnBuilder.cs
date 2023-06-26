namespace Shipwreck.ViewModelUtils.Components;

public class PropertyTableColumnBuilder : TableColumnBuilder
{
    public override TableColumn Build()
    {
        var p = ValueExpression.Body.Type;

        var c = CreateTableColumn();

        c.Header = GetHeader();
        c.SortKey = GetSortKey();
        c.Description = GetDescription();
        c.Icon = GetIcon();
        c.IsReadOnlyDelegate = GetIsReadOnlyDelegate();
        c.IsChangedDelegate = GetIsChangedDelegate();
        c.AdditionalAttributes = GetAdditionalAttributes();

        return c;
    }

    protected virtual TableColumn CreateTableColumn()
    {
        var c = new ReadOnlyTableColumn();
        c.GetValueDelegate = GetGetValueDelegate<object>();
        return c;
    }
}
