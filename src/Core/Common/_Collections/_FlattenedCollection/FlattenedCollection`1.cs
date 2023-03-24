using System.Collections;
using System.Collections.Specialized;

namespace Shipwreck.ViewModelUtils;

public abstract class FlattenedCollection<TParent> : IList, INotifyPropertyChanged, INotifyCollectionChanged
    where TParent : class
{
    private sealed class Entry
    {
        public Entry(TParent parent, object child)
        {
            Parent = parent;
            Child = child;
        }

        public TParent Parent { get; }
        public object Child { get; }

        public override int GetHashCode()
            => Parent.GetHashCode() ^ (Child?.GetHashCode() << 16 ?? 0);

        public override bool Equals(object obj)
            => obj is Entry e
            && e.Parent == Parent
            && e.Child == Child;
    }

    private readonly ObservableCollection<Entry> _Entries;
    private readonly IList<TParent> _Parents;

    protected FlattenedCollection(IList<TParent> parents)
    {
        _Entries = new ObservableCollection<Entry>();
        ((INotifyPropertyChanged)_Entries).PropertyChanged += _Entries_PropertyChanged;
        _Entries.CollectionChanged += _Entries_CollectionChanged;

        _Parents = parents;
        if (parents.Count > 0)
        {
            OnReset();
        }
        ((INotifyCollectionChanged)_Parents).CollectionChanged += _Parents_CollectionChanged;
    }

    public object this[int index]
    {
        get
        {
            var e = _Entries[index];
            return e.Child ?? e.Parent;
        }
    }

    public int Count => _Entries.Count;

    #region IEnumerable

    IEnumerator IEnumerable.GetEnumerator()
    {
        lock (((ICollection)_Entries).SyncRoot)
        {
            foreach (var e in _Entries)
            {
                yield return e.Child ?? e.Parent;
            }
        }
    }

    #endregion IEnumerable

    #region ICollection

    object ICollection.SyncRoot => ((ICollection)_Entries).SyncRoot;
    bool ICollection.IsSynchronized => ((ICollection)_Entries).IsSynchronized;

    void ICollection.CopyTo(System.Array array, int index)
    {
        lock (((ICollection)_Entries).SyncRoot)
        {
            for (var i = 0; i < _Entries.Count; i++)
            {
                var e = _Entries[i];
                array.SetValue(e.Child ?? e.Parent, i + index);
            }
        }
    }

    #endregion ICollection

    #region IList

    object IList.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    bool IList.IsFixedSize => false;
    bool IList.IsReadOnly => true;

    public int IndexOf(object value)
    {
        lock (((ICollection)_Entries).SyncRoot)
        {
            for (var i = 0; i < _Entries.Count; i++)
            {
                var e = _Entries[i];
                if ((value != null && e.Child == value) || e.Parent == value)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    int IList.Add(object value) => throw new NotSupportedException();

    void IList.Clear() => throw new NotSupportedException();

    bool IList.Contains(object value) => IndexOf(value) >= 0;

    void IList.Insert(int index, object value) => throw new NotSupportedException();

    void IList.Remove(object value) => throw new NotSupportedException();

    void IList.RemoveAt(int index) => throw new NotSupportedException();

    #endregion IList

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    private void _Entries_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_Entries.Count) || e.PropertyName == "[]")
        {
            PropertyChanged?.Invoke(this, e);
        }
    }

    #endregion INotifyPropertyChanged

    #region INotifyCollectionChanged

    public event NotifyCollectionChangedEventHandler CollectionChanged;

    private void _Entries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        var ne = e;
        if (e.OldItems?.Count > 0)
        {
            if (e.NewItems?.Count > 0)
            {
                ne = new NotifyCollectionChangedEventArgs(e.Action, ConvertItems(e.NewItems), ConvertItems(e.OldItems), e.NewStartingIndex);
            }
            else
            {
                ne = new NotifyCollectionChangedEventArgs(e.Action, ConvertItems(e.OldItems), e.OldStartingIndex);
            }
        }
        else if (e.NewItems?.Count > 0)
        {
            ne = new NotifyCollectionChangedEventArgs(e.Action, ConvertItems(e.NewItems), e.NewStartingIndex);
        }
        CollectionChanged?.Invoke(this, ne);
    }

    private object[] ConvertItems(IList items)
    {
        var r = new object[items.Count];
        for (var i = 0; i < r.Length; i++)
        {
            var e = (Entry)items[i];
            r[i] = e.Child ?? e.Parent;
        }
        return r;
    }

    #endregion INotifyCollectionChanged

    protected abstract int GetChildCount(TParent parent);

    protected abstract object GetChild(TParent parent, int childIndex);

    private ConditionalWeakTable<TParent, NotifyCollectionChangedEventHandler> _ChildHandlers = new ConditionalWeakTable<TParent, NotifyCollectionChangedEventHandler>();

    protected void AttachCollectionChanged(TParent parent, INotifyCollectionChanged collection)
    {
        var h = _ChildHandlers.GetValue(parent, p => (s, e) => OnChildCollectionChanged(p, e));
        collection.CollectionChanged += h;
    }

    protected void DetachCollectionChanged(TParent parent, INotifyCollectionChanged collection)
    {
        if (_ChildHandlers.TryGetValue(parent, out var h))
        {
            collection.CollectionChanged -= h;
        }
    }

    protected abstract void AttachChildrenChanged(TParent parent);

    protected abstract void DetachChildrenChanged(TParent parent);

    private void _Parents_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems?.Count > 0)
                {
                    lock (((ICollection)_Entries).SyncRoot)
                    {
                        var si = GetIndexForInsert(e);

                        foreach (TParent p in e.NewItems)
                        {
                            si = InsertParent(si, p);
                        }
                    }
                    AssertEntries();
                    return;
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems?.Count > 0)
                {
                    lock (((ICollection)_Entries).SyncRoot)
                    {
                        foreach (TParent p in e.OldItems)
                        {
                            RemoveParent(p);
                        }
                    }
                    AssertEntries();
                    return;
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                if (e.NewItems?.Count > 0 && e.OldItems?.Count > 0)
                {
                    lock (((ICollection)_Entries).SyncRoot)
                    {
                        var si = int.MaxValue;
                        foreach (TParent p in e.OldItems)
                        {
                            var i = RemoveParent(p);
                            if (i >= 0)
                            {
                                si = Math.Min(i, si);
                            }
                        }

                        si = Math.Min(si, Count);

                        foreach (TParent p in e.NewItems)
                        {
                            si = InsertParent(si, p);
                        }
                    }
                    AssertEntries();
                    return;
                }
                break;

            case NotifyCollectionChangedAction.Move:
                if (e.NewItems?.Count > 0 && e.OldItems?.Count > 0)
                {
                    lock (((ICollection)_Entries).SyncRoot)
                    {
                        var si = GetIndexForInsert(e);

                        foreach (TParent p in e.OldItems)
                        {
                            var ri = RemoveParent(p);
                            if (ri >= 0 && ri < si)
                            {
                                si -= GetChildCount(p) + 1;
                            }
                        }
                        si = Math.Max(0, Math.Min(si, Count));

                        foreach (TParent p in e.NewItems)
                        {
                            si = InsertParent(si, p);
                        }
                    }
                    AssertEntries();
                    return;
                }
                break;
        }
        OnReset();
    }

    private int GetIndexForInsert(NotifyCollectionChangedEventArgs e)
        => e.NewStartingIndex > 0
            && IndexOf(_Parents[e.NewStartingIndex - 1]) is int i
            && i >= 0 ? i + 1 + GetChildCount(_Parents[e.NewStartingIndex - 1])
            : e.NewStartingIndex == 0 ? 0
            : Count;

    private void OnReset()
    {
        lock (((ICollection)_Entries).SyncRoot)
        {
            for (var i = _Entries.Count - 1; i >= 0; i--)
            {
                var e = _Entries[i];
                if (e.Child == null)
                {
                    DetachChildrenChanged(e.Parent);
                }
            }

            _Entries.Clear();

            foreach (var p in _Parents)
            {
                InsertParent(Count, p);
            }
        }
    }

    private int InsertParent(int index, TParent parent)
    {
        _Entries.Insert(index++, new Entry(parent, null));
        var cc = GetChildCount(parent);
        for (var i = 0; i < cc; i++)
        {
            _Entries.Insert(index++, new Entry(parent, GetChild(parent, i)));
        }
        AttachChildrenChanged(parent);

        return index;
    }

    private int RemoveParent(TParent parent)
    {
        for (var i = _Entries.Count - 1; i >= 0; i--)
        {
            var e = _Entries[i];
            if (e.Parent == parent)
            {
                _Entries.RemoveAt(i);
                if (e.Child == null)
                {
                    DetachChildrenChanged(parent);
                    return i;
                }
            }
        }
        return -1;
    }

    protected void OnChildCollectionChanged(TParent parent, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems?.Count > 0)
                {
                    for (var i = Count - 1; i >= 0; i--)
                    {
                        var entry = _Entries[i];
                        if (entry.Parent == parent)
                        {
                            if (entry.Child == null)
                            {
                                return;
                            }
                            else if (e.OldItems.Contains(entry.Child))
                            {
                                _Entries.RemoveAt(i);
                            }
                        }
                    }

                    AssertEntries();
                    return;
                }
                break;
        }

        InvalidateParent(parent);
        AssertEntries();
    }

    private void InvalidateParent(TParent parent)
    {
        var i = IndexOf(parent);
        if (i >= 0)
        {
            var cc = GetChildCount(parent);
            var children = new List<object>(cc);
            for (var j = 0; j < cc; j++)
            {
                children.Add(GetChild(parent, j));
            }

            var currentChildren = new List<object>(cc);
            for (var j = i + 1; j < _Entries.Count; j++)
            {
                var ce = _Entries[j];
                if (ce.Parent != parent)
                {
                    break;
                }
                else
                {
                    currentChildren.Add(ce.Child);
                }
            }

            i++;
            while (children.Any() || currentChildren.Any())
            {
                if (children.Any())
                {
                    var fc = children[0];
                    if (fc == currentChildren.FirstOrDefault())
                    {
                        children.RemoveAt(0);
                        currentChildren.RemoveAt(0);
                    }
                    else
                    {
                        var ci = currentChildren.IndexOf(fc);
                        if (ci > 0)
                        {
                            _Entries.RemoveAt(i + ci);
                            currentChildren.RemoveAt(ci);
                        }
                        _Entries.Insert(i, new Entry(parent, fc));
                        children.RemoveAt(0);
                    }
                    i++;
                }
                else
                {
                    _Entries.RemoveAt(i);
                    currentChildren.RemoveAt(0);
                }
            }
        }
    }

    [Conditional("DEBUG")]
    private void AssertEntries()
    {
        var l = new List<Entry>();
        foreach (var p in _Parents)
        {
            l.Add(new Entry(p, null));
            var cc = GetChildCount(p);
            for (var i = 0; i < cc; i++)
            {
                var c = GetChild(p, i);
                if (c is Entry)
                {
                    Debugger.Break();
                }
                l.Add(new Entry(p, c));
            }
        }

        if (!l.SequenceEqual(_Entries))
        {
            Debugger.Break();
        }
    }
}
