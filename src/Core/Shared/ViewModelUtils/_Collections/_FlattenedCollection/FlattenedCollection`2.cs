using System.Collections;
using System.Collections.Specialized;

namespace Shipwreck.ViewModelUtils;

public abstract class FlattenedCollection<TParent, TChild> : FlattenedCollection<TParent>
    where TParent : class
    where TChild : class
{
    protected FlattenedCollection(IList<TParent> parents)
        : base(parents)
    {
    }

    protected abstract INotifyCollectionChanged GetChildren(TParent parent);

    protected sealed override int GetChildCount(TParent parent) => ((IList)GetChildren(parent)).Count;

    protected override void AttachChildrenChanged(TParent parent)
    {
        AttachCollectionChanged(parent, GetChildren(parent));
    }

    protected override void DetachChildrenChanged(TParent parent)
    {
        DetachCollectionChanged(parent, GetChildren(parent));
    }

    protected sealed override object GetChild(TParent parent, int childIndex)
        => ((IList)GetChildren(parent))[childIndex];
}
