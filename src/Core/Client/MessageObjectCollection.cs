namespace Shipwreck.ViewModelUtils.Client;

public abstract class MessageObjectCollection<TMessage, TItem> : Collection<TItem>, IMessageObjectCollection
    where TMessage : FrameworkMessageBase
    where TItem : FrameworkMessageObject
{
    private readonly WeakReference<object> _OwnerOrMessage;

    public MessageObjectCollection(FrameworkMessageObject owner)
    {
        _OwnerOrMessage = new WeakReference<object>(owner);
    }

    public MessageObjectCollection(FrameworkMessageBase message)
    {
        _OwnerOrMessage = new WeakReference<object>(message);
    }

    public FrameworkMessageObject? Owner
        => _OwnerOrMessage.TryGetTarget(out var t) ? t as FrameworkMessageObject : null;

    public TMessage? Message
        => _OwnerOrMessage.TryGetTarget(out var t) ? (t as TMessage) ?? (t as FrameworkMessageObject)?.Message as TMessage : null;

    FrameworkMessageBase? IMessageObjectCollection.Message => Message;

    protected override void ClearItems()
    {
        foreach (var e in this)
        {
            if (e.Collection == this)
            {
                e.Collection = null;
            }
        }
        base.ClearItems();
    }

    protected override void InsertItem(int index, TItem item)
    {
        if (item.Collection != null)
        {
            if (item.Message != null
                && Message != null
                && Message != item.Message)
            {
                throw new ArgumentException();
            }
        }
        item.Collection = this;
        base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        var old = this[index];
        base.RemoveItem(index);
        if (old.Collection == this)
        {
            old.Collection = null;
        }
    }

    protected override void SetItem(int index, TItem item)
    {
        var old = this[index];

        if (old == item)
        {
            return;
        }

        if (item.Collection != null
            && item.Message != null
            && Message != null
            && Message != item.Message)
        {
            throw new ArgumentException();
        }

        item.Collection = this;

        base.SetItem(index, item);
        if (old.Collection == this)
        {
            old.Collection = null;
        }
    }

    public void Set(IEnumerable<TItem> value)
    {
        if (value == this)
        {
            return;
        }

        Clear();

        if (value != null)
        {
            foreach (var v in value)
            {
                Add(v);
            }
        }
    }
}
