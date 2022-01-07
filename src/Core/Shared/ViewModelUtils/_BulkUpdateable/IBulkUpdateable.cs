namespace Shipwreck.ViewModelUtils;

public interface IBulkUpdateable<T> : IFrameworkPageViewModel
    where T : class, IBulkUpdateableItem
{
    string TypeDisplayName { get; }
    BulkUpdateableCollection<T> Items { get; }
    bool IsInEditMode { get; set; }
    bool IsUpdating { get; set; }

    CommandViewModelBase EnterEditModeCommand { get; }
    CommandViewModelBase CommitEditCommand { get; }

    void Refresh();
}
