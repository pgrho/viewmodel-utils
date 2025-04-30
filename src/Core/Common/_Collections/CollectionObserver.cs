using System.Collections.Specialized;

namespace Shipwreck.ViewModelUtils;

public abstract class CollectionObserver<TItem, TValue> : IDisposable
    where TItem : class
{
    #region Value

    private bool _HasValue;
    private TValue? _Value;

    public TValue? Value
    {
        get
        {
            if (!_HasValue)
            {
                Invalidate();
            }
            return _Value;
        }
    }

    public bool Invalidate()
    {
        if (_HasValue)
        {
            var v = Calculate();
            if (Equals(v, _Value))
            {
                _Value = v;
                return false;
            }
            else
            {
                _Value = v;
                OnValueChanged();
                return true;
            }
        }
        else
        {
            _Value = Calculate();
            _HasValue = true;
            return false;
        }
    }

    protected abstract TValue Calculate();

    protected virtual bool Equals(TValue? left, TValue? right)
        => EqualityComparer<TValue?>.Default.Equals(left, right);

    public event EventHandler? ValueChanged;

    protected virtual void OnValueChanged()
        => ValueChanged?.Invoke(this, EventArgs.Empty);

    #endregion Value

    #region Source

    private IEnumerable<TItem>? _Source;

    public IEnumerable<TItem>? Source
    {
        get => _Source;
        set
        {
            var prev = _Source;
            if (value != prev)
            {
                if (prev != null)
                {
                    if (prev is INotifyCollectionChanged ncc)
                    {
                        ncc.CollectionChanged -= Source_CollectionChanged!;
                    }
                    if (prev is INotifyPropertyChanged npc)
                    {
                        npc.PropertyChanged -= Source_PropertyChanged!;
                    }

                    foreach (var e in prev)
                    {
                        OnItemRemoved(e);
                    }
                }
                _Source = value;
                if (value != null)
                {
                    foreach (var e in value)
                    {
                        OnItemAdded(e);
                    }
                    if (value is INotifyCollectionChanged ncc)
                    {
                        ncc.CollectionChanged += Source_CollectionChanged!;
                    }
                    if (value is INotifyPropertyChanged npc)
                    {
                        npc.PropertyChanged += Source_PropertyChanged!;
                    }
                }
            }
        }
    }

    protected virtual void OnItemAdded(TItem item)
    {
        if (item is INotifyPropertyChanged n)
        {
            n.PropertyChanged -= Item_PropertyChanged!;
            n.PropertyChanged += Item_PropertyChanged!;
        }
    }

    protected virtual void OnItemRemoved(TItem item)
    {
        if (item is INotifyPropertyChanged n)
        {
            n.PropertyChanged -= Item_PropertyChanged!;
        }
    }

    private void Source_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (_Source != sender)
        {
            return;
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                {
                    foreach (TItem m in e.NewItems)
                    {
                        OnItemAdded(m);
                    }
                    Invalidate();
                    return;
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems != null)
                {
                    foreach (TItem m in e.OldItems)
                    {
                        OnItemRemoved(m);
                    }
                    Invalidate();
                    return;
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                if (e.OldItems != null)
                {
                    foreach (TItem m in e.OldItems)
                    {
                        OnItemRemoved(m);
                    }

                    if (e.NewItems != null)
                    {
                        foreach (TItem m in e.NewItems)
                        {
                            OnItemAdded(m);
                        }
                        Invalidate();
                        return;
                    }
                }
                break;

            case NotifyCollectionChangedAction.Move:
                Invalidate();
                return;
        }

        foreach (var m in Source ?? [])
        {
            OnItemAdded(m);
        }
        Invalidate();
    }

    private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (OnItemPropertyChanged(e.PropertyName!))
        {
            Invalidate();
        }
    }

    private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (OnSourcePropertyChanged(e.PropertyName!))
        {
            Invalidate();
        }
    }

    protected virtual bool OnItemPropertyChanged(string propertyName)
        => true;

    protected virtual bool OnSourcePropertyChanged(string propertyName)
        => propertyName != nameof(IList<object>.Count) && propertyName != "[]";

    #endregion Source

    #region IDisposable

    protected bool IsDisposed { get; set; }

    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                ValueChanged = null;
                Source = null;
            }

            IsDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    #endregion IDisposable
}
