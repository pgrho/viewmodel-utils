using System.Collections;
using System.Collections.Specialized;

namespace Shipwreck.ViewModelUtils;

public static class BulkUpdateableCollection
{
    private sealed class DefaultUpdaterFactor : ICollectionBulkUpdaterFactory
    {
        public BulkUpdateableCollection<T>.CollectionBulkUpdater Create<T>() where T : class
            => new BulkUpdateableCollection<T>.CollectionBulkUpdater();
    }

    public static ICollectionBulkUpdaterFactory UpdaterFactory { get; set; } = new DefaultUpdaterFactor();
}

public class BulkUpdateableCollection<T> : ObservableCollection<T>
    where T : class
{
    public class CollectionBulkUpdater
    {
        public void Set(BulkUpdateableCollection<T> collection, IEnumerable<T> items)
        {
            if (items != collection)
            {
                lock (((ICollection)collection).SyncRoot)
                {
                    var list = items as IReadOnlyList<T> ?? items?.ToList();
                    if (list?.Count > 0)
                    {
                        OnReplace(collection, list);
                    }
                    else if (collection.Items.Any())
                    {
                        OnClear(collection);
                    }
                }
            }
        }

        protected static bool TryInsert(BulkUpdateableCollection<T> collection, IReadOnlyList<T> items)
        {
            if (items.Count > collection.Count && collection.Count > 0)
            {
                var si = 0;
                foreach (var ce in collection)
                {
                    var found = false;
                    for (var i = si; i < items.Count; i++)
                    {
                        if (items[i] == ce)
                        {
                            si = i + 1;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        return false;
                    }
                }

                for (var i = 0; i < items.Count; i++)
                {
                    var e = items[i];
                    if (collection.ElementAtOrDefault(i) != e)
                    {
                        collection.Insert(i, e);
                    }
                }
                return true;
            }
            return false;
        }

        protected static bool TryRemove(BulkUpdateableCollection<T> collection, IReadOnlyList<T> items)
        {
            if (0 < items.Count && items.Count < collection.Count)
            {
                var si = 0;
                foreach (var e in items)
                {
                    var found = false;
                    for (var i = si; i < collection.Count; i++)
                    {
                        if (collection[i] == e)
                        {
                            si = i + 1;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        return false;
                    }
                }

                for (var i = collection.Count - 1; i >= 0; i--)
                {
                    if (collection[i] != items.ElementAtOrDefault(items.Count - collection.Count + i))
                    {
                        collection.RemoveAt(i);
                    }
                }

                return true;
            }
            return false;
        }

        protected virtual void OnReplace(BulkUpdateableCollection<T> collection, IReadOnlyList<T> items)
        {
            if (TryInsert(collection, items)
                || TryRemove(collection, items))
            {
                return;
            }

            collection.Clear();

            foreach (var e in items)
            {
                collection.Add(e);
            }
        }

        protected virtual void OnClear(BulkUpdateableCollection<T> collection)
            => collection.Clear();

        public virtual int RemoveAll(BulkUpdateableCollection<T> collection, Func<T, bool> predicate)
        {
            lock (((ICollection)collection).SyncRoot)
            {
                var c = 0;
                for (var i = collection.Count - 1; i >= 0; i--)
                {
                    if (predicate(collection[i]))
                    {
                        collection.RemoveAt(i);
                        c++;
                    }
                }
                return c;
            }
        }
    }

    public sealed class ClearCollectionBulkUpdater : CollectionBulkUpdater
    {
        protected override void OnReplace(BulkUpdateableCollection<T> collection, IReadOnlyList<T> items)
        {
            collection.Clear();

            foreach (var e in items)
            {
                collection.Items.Add(e);
            }

            collection.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items as IList ?? items.ToList(), 0));
        }
    }

    public sealed class ResetCollectionBulkUpdater : CollectionBulkUpdater
    {
        protected override void OnReplace(BulkUpdateableCollection<T> collection, IReadOnlyList<T> items)
        {
            collection.Items.Clear();

            foreach (var e in items)
            {
                collection.Items.Add(e);
            }

            collection.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void OnClear(BulkUpdateableCollection<T> collection)
        {
            collection.Clear();
            collection.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public override int RemoveAll(BulkUpdateableCollection<T> collection, Func<T, bool> predicate)
        {
            lock (((ICollection)collection).SyncRoot)
            {
                var c = 0;
                for (var i = collection.Count - 1; i >= 0; i--)
                {
                    if (predicate(collection[i]))
                    {
                        collection.Items.RemoveAt(i);
                        c++;
                    }
                }
                if (c > 0)
                {
                    collection.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
                return c;
            }
        }
    }

    private readonly CollectionBulkUpdater _Updater;

    public BulkUpdateableCollection(CollectionBulkUpdater? updater = null)
    {
        _Updater = updater ?? BulkUpdateableCollection.UpdaterFactory.Create<T>();
    }

    public BulkUpdateableCollection(IEnumerable<T> items, CollectionBulkUpdater? updater = null)
        : base(items.ToList())
    {
        _Updater = updater ?? BulkUpdateableCollection.UpdaterFactory.Create<T>();
    }

    public new event PropertyChangedEventHandler PropertyChanged
    {
        add => base.PropertyChanged += value;
        remove => base.PropertyChanged -= value;
    }

    public void Set(IEnumerable<T> items)
        => _Updater.Set(this, items);

    public bool SetIfNeeded(IReadOnlyList<T> items)
    {
        if (!items.SequenceEqual(this))
        {
            Set(items);
            return true;
        }
        return false;
    }

    public int RemoveAll(Func<T, bool> predicate)
    {
        var c = 0;
        for (var i = Count - 1; i >= 0; i--)
        {
            if (predicate(this[i]))
            {
                RemoveAt(i);
                c++;
            }
        }
        return c;
    }

    protected bool SetProperty(ref string? field, string? value, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        if (value != field)
        {
            field = value;
            RaisePropertyChanged(propertyName);
            onChanged?.Invoke();
            return true;
        }
        return false;
    }

    protected bool SetProperty<TValue>(ref TValue field, TValue value, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        if (!((field as IEquatable<TValue>)?.Equals(value) ?? Equals(field, value)))
        {
            field = value;
            RaisePropertyChanged(propertyName);

            onChanged?.Invoke();
            return true;
        }
        return false;
    }

    protected bool SetFlagProperty(ref byte field, byte flag, bool hasFlag, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        var nv = (byte)(hasFlag ? (field | flag) : (field & ~flag));
        return SetProperty(ref field, nv, onChanged: onChanged, propertyName: propertyName);
    }

    protected bool SetFlagProperty(ref ushort field, ushort flag, bool hasFlag, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        var nv = (ushort)(hasFlag ? (field | flag) : (field & ~flag));
        return SetProperty(ref field, nv, onChanged: onChanged, propertyName: propertyName);
    }

    protected bool SetFlagProperty(ref uint field, uint flag, bool hasFlag, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        var nv = hasFlag ? (field | flag) : (field & ~flag);
        return SetProperty(ref field, nv, onChanged: onChanged, propertyName: propertyName);
    }

    protected bool SetFlagProperty(ref ulong field, ulong flag, bool hasFlag, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        var nv = hasFlag ? (field | flag) : (field & ~flag);
        return SetProperty(ref field, nv, onChanged: onChanged, propertyName: propertyName);
    }
    protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var e in items)
        {
            Add(e);
        }
    }
}
