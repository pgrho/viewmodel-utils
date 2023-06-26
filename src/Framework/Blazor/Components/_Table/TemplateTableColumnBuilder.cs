namespace Shipwreck.ViewModelUtils.Components;

public sealed class TemplateTableColumnBuilder : TableColumnBuilder
{
    private readonly Type _ComponentType;

    public TemplateTableColumnBuilder(Type componentType)
    {
        _ComponentType = componentType;
    }

    public override TableColumn Build()
    {
        var p = ValueExpression.Body.Type;

        var c = new TemplateTableColumn(_ComponentType);

        c.Header = GetHeader();
        c.SortKey = GetSortKey();
        c.Description = GetDescription();
        c.Icon = GetIcon();
        c.IsReadOnlyDelegate = GetIsReadOnlyDelegate();
        c.IsChangedDelegate = GetIsChangedDelegate();
        c.AdditionalAttributes = GetAdditionalAttributes();

        return c;
    }
}
