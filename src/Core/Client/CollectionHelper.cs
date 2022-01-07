using System.Collections.ObjectModel;

namespace Shipwreck.ViewModelUtils.Client;

internal static class CollectionHelper
{
    public static Collection<T> GetOrCreate<T>(ref Collection<T> collection)
        => collection ??= new Collection<T>();

    public static void Set<T>(ref Collection<T> collection, IEnumerable<T> items)
    {
        if (items == collection)
        {
            return;
        }
        collection?.Clear();
        if (items != null)
        {
            foreach (var item in items)
            {
                (collection = GetOrCreate(ref collection)).Add(item);
            }
        }
    }
}
