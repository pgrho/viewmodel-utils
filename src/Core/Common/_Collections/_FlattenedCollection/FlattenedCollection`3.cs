using System.Collections;
using System.Collections.Specialized;

namespace Shipwreck.ViewModelUtils;

public abstract class FlattenedCollection<TParent, TChild1, TChild2> : FlattenedCollection<TParent>
    where TParent : class
    where TChild1 : class
    where TChild2 : class
{
    protected FlattenedCollection(IList<TParent> parents)
        : base(parents)
    {
    }

    protected abstract INotifyCollectionChanged GetChildren1(TParent parent);

    protected abstract INotifyCollectionChanged GetChildren2(TParent parent);

    protected sealed override int GetChildCount(TParent parent) => ((ICollection)GetChildren1(parent)).Count + ((ICollection)GetChildren2(parent)).Count;

    protected override void AttachChildrenChanged(TParent parent)
    {
        AttachCollectionChanged(parent, GetChildren1(parent));
        AttachCollectionChanged(parent, GetChildren2(parent));
    }

    protected override void DetachChildrenChanged(TParent parent)
    {
        DetachCollectionChanged(parent, GetChildren1(parent));
        DetachCollectionChanged(parent, GetChildren2(parent));
    }

    protected sealed override object GetChild(TParent parent, int childIndex)
    {
        var c1 = (IList)GetChildren1(parent);
        if (childIndex < c1.Count)
        {
            return c1[childIndex];
        }
        return ((IList)GetChildren2(parent))[childIndex - c1.Count];
    }
}
