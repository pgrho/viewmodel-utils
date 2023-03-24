namespace Shipwreck.ViewModelUtils;

public interface ICollectionBulkUpdaterFactory
{
    BulkUpdateableCollection<T>.CollectionBulkUpdater Create<T>()
        where T : class;
}
