namespace Shipwreck.ViewModelUtils;

public interface IHasExtensionColumns : IHasColumns
{
    BulkUpdateableCollection<string> ExtensionColumns { get; }
    IReadOnlyList<string> SelectedExtensionColumns { get; set; }

    IEnumerable<string> GetDefaultExtensionColumns();

    string GetColumnDisplayName(string columnName)
#if NET7_0_OR_GREATER
        => null
#endif
        ;
}
