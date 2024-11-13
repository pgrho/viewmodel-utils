namespace Shipwreck.ViewModelUtils;

public interface IHasExtensionColumns : IHasColumns
{
    BulkUpdateableCollection<string> ExtensionColumns { get; }
    IReadOnlyList<string> SelectedExtensionColumns { get; set; }

    IEnumerable<string> GetDefaultExtensionColumns();

    string GetColumnDisplayName(string columnName)
#if NET9_0_OR_GREATER
        => null
#endif
        ;
}
