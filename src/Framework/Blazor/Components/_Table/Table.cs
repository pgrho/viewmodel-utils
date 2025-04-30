namespace Shipwreck.ViewModelUtils.Components;

public sealed class Table
{
    private RenderFragment _HeaderRenderer;

    internal Table(IEnumerable<TableColumn> columns)
    {
        Columns = columns.ToArray().AsReadOnly();
    }

    public ReadOnlyCollection<TableColumn> Columns { get; }

    public RenderFragment RenderHeader(object dataContext)
        => _HeaderRenderer ??= b => Columns.RenderHeader(dataContext, b);

    public RenderFragment RenderCell(object dataContext)
        => b => Columns.RenderCell(dataContext, b);
}
