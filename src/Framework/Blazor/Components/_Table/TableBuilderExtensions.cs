using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Components;

public static class TableBuilderExtensions
{
    public static T IsReadOnly<T>(this T builder, bool? isReadOnly)
        where T : TableBuilder
    {
        builder.DefaultIsReadOnly = isReadOnly;

        return builder;
    }

    public static TableBuilder<T> IsReadOnly<T>(this TableBuilder<T> builder, Expression<Func<T, bool>> isReadOnly)
    {
        builder.DefaultIsReadOnly = null;
        builder.DefaultIsReadOnlyExpression = isReadOnly;

        return builder;
    }

    public static T AddSelection<T>(this T builder)
        where T : TableBuilder
    {
        builder.Columns.Add(new SelectionTableColumnBuilder()
        {
            TableBuilder = builder
        });

        return builder;
    }

    public static TableBuilder<T> AddProperty<T, TProperty>(
        this TableBuilder<T> builder,
        Expression<Func<T, TProperty>> valueExpression,
        string header = null,
        string sortKey = null,
        string description = null,
        string icon = null,
        float? width = null,
        bool? isReadOnly = null, Expression<Func<T, bool>> isReadOnlyExpression = null,
        Expression<Func<T, bool>> isChangedExpression = null,
        IEnumerable<TProperty> options = null, Expression<Func<T, IEnumerable<TProperty>>> optionsExpression = null)
    {
        var t = typeof(TProperty);

        var b = t == typeof(bool) ? new BooleanTableColumnBuilder()
            : t == typeof(string) ? new StringTableColumnBuilder()
            : t == typeof(int) ? new Int32TableColumnBuilder()
            : t == typeof(int?) ? new NullableInt32TableColumnBuilder()
            : t == typeof(short) ? new Int16TableColumnBuilder()
            : t == typeof(short?) ? new NullableInt16TableColumnBuilder()
            : t == typeof(long) ? new Int64TableColumnBuilder()
            : t == typeof(long?) ? new NullableInt64TableColumnBuilder()
            : t == typeof(byte) ? new ByteTableColumnBuilder()
            : t == typeof(byte?) ? new NullableByteTableColumnBuilder()
            : t == typeof(DateTimeOffset) ? new DateTimeOffsetTableColumnBuilder()
            : t == typeof(DateTimeOffset?) ? new DateTimeOffsetTableColumnBuilder()
            : t.IsAssignableTo(typeof(IEntitySelector)) ? new EntitySelectorTableColumnBuilder()
            : (TableColumnBuilder)new PropertyTableColumnBuilder();

        b.TableBuilder = builder;

        b.ValueExpression = valueExpression;

        b.Options = options;
        b.OptionsExpression = optionsExpression;

        b.Header = header;
        b.SortKey = sortKey;
        b.Description = description;
        b.Icon = icon;
        b.Width = width;

        b.IsReadOnly = isReadOnly;
        b.IsReadOnlyExpression = isReadOnlyExpression;

        b.IsChangedExpression = isChangedExpression;

        builder.Columns.Add(b);

        return builder;
    }

    public static TableBuilder<T> AddEnum<T, TProperty>(this TableBuilder<T> builder, Expression<Func<T, TProperty>> valueExpression, string header = null, string sortKey = null, string description = null, string icon = null, float? width = null, bool? isReadOnly = null, Expression<Func<T, bool>> isReadOnlyExpression = null, Expression<Func<T, bool>> isChangedExpression = null, Expression<Func<T, IEnumerable<TProperty>>> optionsExpression = null)
        where TProperty : struct, Enum
    {
        var b = new EnumTableColumnBuilder<TProperty>();

        b.TableBuilder = builder;

        b.ValueExpression = valueExpression;
        b.OptionsExpression = optionsExpression;

        b.Header = header;
        b.SortKey = sortKey;
        b.Description = description;
        b.Icon = icon;
        b.Width = width;

        b.IsReadOnly = isReadOnly;
        b.IsReadOnlyExpression = isReadOnlyExpression;

        b.IsChangedExpression = isChangedExpression;

        builder.Columns.Add(b);

        return builder;
    }

    public static TableBuilder<T> AddTemplate<T, TProperty>(this TableBuilder<T> builder, Type componentType, Expression<Func<T, TProperty>> valueExpression, string header = null, string sortKey = null, string description = null, string icon = null, float? width = null, bool? isReadOnly = null, Expression<Func<T, bool>> isReadOnlyExpression = null, Expression<Func<T, bool>> isChangedExpression = null)
    {
        var b = new TemplateTableColumnBuilder(componentType);

        b.TableBuilder = builder;

        b.ValueExpression = valueExpression;

        b.Header = header;
        b.SortKey = sortKey;
        b.Description = description;
        b.Icon = icon;
        b.Width = width;

        b.IsReadOnly = isReadOnly;
        b.IsReadOnlyExpression = isReadOnlyExpression;

        b.IsChangedExpression = isChangedExpression;

        builder.Columns.Add(b);

        return builder;
    }

    public static void RenderHeader(this ReadOnlyCollection<TableColumn> columns, object dataContext, RenderTreeBuilder builder)
    {
        var i = 1;
        foreach (var column in columns)
        {
            builder.AddContent(i++, b => column.RenderHeader(b, dataContext));
        }
    }

    public static void RenderCell<T>(this ReadOnlyCollection<TableColumn> columns, T dataContext, RenderTreeBuilder builder)
    {
        var i = 1;
        foreach (var column in columns)
        {
            builder.AddContent(i++, b => column.RenderCell(b, dataContext));
        }
    }
}
