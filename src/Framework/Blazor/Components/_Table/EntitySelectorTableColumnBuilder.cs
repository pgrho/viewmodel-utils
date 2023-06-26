namespace Shipwreck.ViewModelUtils.Components;

public class EntitySelectorTableColumnBuilder : TableColumnBuilder
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
        var c = new EntitySelectorTableColumn();
        c.GetValueDelegate = GetGetValueDelegate<IEntitySelector>();
        return c;
    }

    protected override string GetSortKey()
    {
        var bsk = base.GetSortKey();
        if (SortKey == null && !string.IsNullOrEmpty(bsk))
        {
            return bsk + ".SelectedItem.Code";
        }
        return bsk;
    }
}
