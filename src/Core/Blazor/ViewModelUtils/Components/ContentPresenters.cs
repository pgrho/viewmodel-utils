using System.Collections;

namespace Shipwreck.ViewModelUtils.Components;

public sealed class ContentPresenters : ComponentBase, ICollection<ContentPresenter>
{
    private List<ContentPresenter> _Items;

    private List<ContentPresenter> Items => _Items ??= new List<ContentPresenter>();

    public void Insert(int index, ContentPresenter item)
    {
        Items.Insert(index, item);
        StateHasChanged();
    }

    #region ICollection

    bool ICollection<ContentPresenter>.IsReadOnly => false;
    public int Count => _Items?.Count ?? 0;

    public void Add(ContentPresenter item)
    {
        Items.Add(item);
        StateHasChanged();
    }

    public bool Remove(ContentPresenter item)
    {
        if (_Items?.Remove(item) == true)
        {
            StateHasChanged();
            return true;
        }
        return false;
    }

    public void Clear()
    {
        if (_Items?.Count > 0)
        {
            var old = _Items;
            _Items = null;
            StateHasChanged();
            foreach (var c in old)
            {
                (c.View as IDisposable)?.Dispose();
            }
        }
    }

    public bool Contains(ContentPresenter item)
        => _Items?.Contains(item) ?? false;

    public void CopyTo(ContentPresenter[] array, int arrayIndex)
    {
        if (_Items != null)
        {
            _Items.CopyTo(array, arrayIndex);
        }
    }

    #endregion ICollection

    #region IEnumerable

    public List<ContentPresenter>.Enumerator GetEnumerator() => Items.GetEnumerator();

    IEnumerator<ContentPresenter> IEnumerable<ContentPresenter>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion IEnumerable

    #region Remove

    public bool Remove(Type componentType)
        => Remove(e => e.ViewType == componentType);

    public bool Remove(ComponentBase component)
        => Remove(e => e.View == component);

    public bool Remove(Predicate<ContentPresenter> predicate)
    {
        var item = _Items?.FindLast(predicate);
        return item != null && Remove(item);
    }

    public int RemoveAll(Type componentType)
        => RemoveAll(e => e.ViewType == componentType);

    public int RemoveAll(ComponentBase component)
        => RemoveAll(e => e.View == component);

    public int RemoveAll(Predicate<ContentPresenter> predicate)
    {
        var c = 0;
        if (_Items != null)
        {
            for (var i = _Items.Count - 1; i >= 0; i--)
            {
                if (predicate(_Items[i]))
                {
                    _Items.RemoveAt(i);
                    ++c;
                }
            }
            if (c > 0)
            {
                StateHasChanged();
            }
        }

        return c;
    }

    #endregion Remove

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (_Items != null)
        {
            var i = 0;
            foreach (var e in _Items)
            {
                builder.OpenComponent(++i, typeof(ContentPresenter));
                builder.AddAttribute(++i, nameof(ContentPresenter.ViewType), e.ViewType);
                builder.AddAttribute(++i, nameof(ContentPresenter.Attributes), e.Attributes);
                builder.AddComponentReferenceCapture(++i, o => e.View = o as ComponentBase);
                builder.SetKey(e.GetHashCode());
                builder.CloseComponent();
            }
        }
    }
}
