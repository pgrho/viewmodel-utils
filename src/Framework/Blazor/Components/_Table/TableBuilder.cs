using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Components;

public class TableBuilder
{
    public bool? DefaultIsReadOnly { get; set; }
    public LambdaExpression DefaultIsReadOnlyExpression { get; set; }

    public List<TableColumnBuilder> Columns { get; } = new();

    public Table Build()
        => new(Columns.Select(e => e.Build()));
}

public sealed class TableBuilder<T> : TableBuilder
{
}
