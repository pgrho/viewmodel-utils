namespace Shipwreck.ViewModelUtils.Components;

internal sealed class DateTimeOffsetTableColumnBuilder : PropertyTableColumnBuilder
{
    protected override TableColumn CreateTableColumn()
        => new DateTimeOffsetTableColumn()
        {
            GetValueDelegate = GetGetValueDelegate<DateTimeOffset?>()
        };
}
