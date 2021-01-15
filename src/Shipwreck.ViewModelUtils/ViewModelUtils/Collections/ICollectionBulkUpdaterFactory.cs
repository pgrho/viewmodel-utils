namespace Shipwreck.ViewModelUtils.Collections
{
    public interface ICollectionBulkUpdaterFactory
    {
        BulkUpdateableCollection<T>.CollectionBulkUpdater Create<T>()
            where T : class;
    }
}
