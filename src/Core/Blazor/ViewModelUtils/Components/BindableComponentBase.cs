using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Shipwreck.ViewModelUtils.JSInterop;

namespace Shipwreck.ViewModelUtils.Components
{
    public partial class WrapPanel<T> : ItemsControl<T>
        where T : class
    {
        protected override int ColumnCount => _Columns;
        private int _Columns = 1;

        private int SetColumnCount(ScrollInfo info)
            => _Columns = Math.Max((int)Math.Floor(info.ClientWidth / ItemWidth), 1);

        #region ItemWidth

        private float _DefaultItemWidth = 100;
        private float? _MinItemWidth;

        [Parameter]
        public float DefaultItemWidth
        {
            get => _DefaultItemWidth;
            set => SetProperty(ref _DefaultItemWidth, value);
        }

        protected float ItemWidth => _MinItemWidth ?? DefaultItemWidth;

        #endregion ItemWidth

        #region ItemHeight

        private float _DefaultItemHeight = 100;
        private float? _MinItemHeight;

        [Parameter]
        public float DefaultItemHeight
        {
            get => _DefaultItemHeight;
            set => SetProperty(ref _DefaultItemHeight, value);
        }

        protected float ItemHeight => _MinItemHeight ?? DefaultItemHeight;

        #endregion ItemHeight

        protected override void SetControlInfo(ItemsControlScrollInfo info, bool forceScroll, int? firstIndex = null)
        {
            SetColumnCount(info.Viewport);
            _MinItemWidth = info.MinWidth > 0 ? info?.MinWidth : null;
            _MinItemHeight = info.MinHeight > 0 ? info?.MinHeight : null;

            if (firstIndex == null)
            {
                int fi;
                float ft;
                if (info.First != null)
                {
                    ft = info.Viewport.ScrollTop - info.First.Top;

                    var w = info.First.LastIndex + 1 - info.First.FirstIndex;
                    if (w == 1)
                    {
                        fi = info.First.FirstIndex;
                    }
                    else
                    {
                        var rows = (w - 1) / ColumnCount + 1;
                        var rh = info.First.Height / rows;
                        var ri = Math.Min(Math.Max(0, (int)Math.Floor(ft / rh)), rows - 1);
                        fi = info.First.FirstIndex + ri * ColumnCount;
                        ft -= rh * ri;
                    }
                }
                else
                {
                    fi = 0;
                    ft = 0;
                }

                UpdateRange(info.Viewport, fi, ft, forceScroll);
            }
            else
            {
                UpdateRange(info.Viewport, firstIndex.Value, 0, true);
            }
        }

        protected override void UpdateRange(ScrollInfo info, int firstIndex, float localY, bool forceScroll)
        {
            SetColumnCount(info);

            var r = Math.Max((int)Math.Ceiling((info.ClientHeight + localY) / ItemHeight), 1);

            SetVisibleRange(firstIndex, firstIndex + ColumnCount * r - 1, localY, forceScroll);
        }

        #region BuildRenderTree

        [Parameter]
        public string TagName { get; set; }

        protected override string GetTagName() => TagName ?? base.GetTagName();

        protected override void GetRenderingRange(ref int firstIndex, ref int lastIndex)
        {
            base.GetRenderingRange(ref firstIndex, ref lastIndex);

            if (ColumnCount > 1)
            {
                firstIndex = (firstIndex / ColumnCount) * ColumnCount;
            }
        }

        protected override void RenderFirstPadding(RenderTreeBuilder builder, ref int sequence, int firstIndex)
        {
            float height;
            if (firstIndex <= 0)
            {
                height = 0;
            }
            else
            {
                var el = Lines.FirstOrDefault(e => e.FirstIndex <= firstIndex);
                if (el != null)
                {
                    var cl = ColumnCount;
                    var rows = 1 + (el.LastIndex - el.FirstIndex) / cl;
                    var rowIndex = (firstIndex - el.FirstIndex) / cl;
                    height = el.Top + el.Height * rowIndex / rows;
                }
                else
                {
                    height = ((firstIndex + ColumnCount - 1) / ColumnCount) * ItemHeight;
                }
            }

            RenderPaddingCore(
                  builder,
                  ref sequence,
                  0,
                  firstIndex - 1,
                  Math.Max(0, height));
        }

        protected override void RenderLastPadding(RenderTreeBuilder builder, ref int sequence, int lastIndex)
            => RenderPaddingCore(
                builder,
                ref sequence,
                lastIndex + 1,
                Source.Count - 1,
                Math.Max(0, (Source.Count - lastIndex + ColumnCount - 2) / ColumnCount * ItemHeight));

        #endregion BuildRenderTree
    }

    public partial class StackPanel<T> : ItemsControl<T>
        where T : class
    {
        protected override int ColumnCount => 1;

        #region ItemHeight

        private float _DefaultItemHeight = 100;
        private float? _MinItemHeight;

        [Parameter]
        public float DefaultItemHeight
        {
            get => _DefaultItemHeight;
            set => SetProperty(ref _DefaultItemHeight, value);
        }

        protected float ItemHeight => _MinItemHeight ?? DefaultItemHeight;

        #endregion ItemHeight

        protected override void SetControlInfo(ItemsControlScrollInfo info, bool forceScroll, int? firstIndex = null)
        {
            _MinItemHeight = info.MinHeight > 0 ? info?.MinHeight : null;
            if (firstIndex == null)
            {
                int fi;
                float ft;
                if (info.First != null)
                {
                    ft = info.Viewport.ScrollTop - info.First.Top;

                    var c = Math.Max(1, info.First.LastIndex + 1 - info.First.FirstIndex);
                    if (c == 1)
                    {
                        fi = info.First.FirstIndex;
                    }
                    else
                    {
                        var li = (int)Math.Floor(ft * c / info.First.Height);
                        fi = info.First.FirstIndex + li;
                        ft -= info.First.Height * li / c;
                    }
                }
                else
                {
                    fi = 0;
                    ft = 0;
                }

                UpdateRange(info.Viewport, fi, ft, forceScroll);
            }
            else
            {
                UpdateRange(info.Viewport, firstIndex.Value, 0, true);
            }
        }

        protected override void UpdateRange(ScrollInfo info, int firstIndex, float localY, bool forceScroll)
        {
            var r = Math.Max((int)Math.Ceiling((info.ClientHeight + localY) / ItemHeight), 1);

            SetVisibleRange(firstIndex, firstIndex + r - 1, localY, forceScroll);
        }

        #region BuildRenderTree

        [Parameter]
        public string TagName { get; set; }

        protected override string GetTagName() => TagName ?? base.GetTagName();

        protected override void RenderFirstPadding(RenderTreeBuilder builder, ref int sequence, int firstIndex)
        {
            float height;
            if (firstIndex <= 0)
            {
                height = 0;
            }
            else
            {
                var el = Lines.FirstOrDefault(e => e.FirstIndex <= firstIndex);
                if (el != null)
                {
                    height = el.Top + el.Height * (firstIndex - el.FirstIndex) / (el.LastIndex - el.FirstIndex + 1);
                }
                else
                {
                    height = firstIndex * ItemHeight;
                }
            }

            RenderPaddingCore(
                  builder,
                  ref sequence,
                  0,
                  firstIndex - 1,
                  Math.Max(0, height));
        }

        protected override void RenderLastPadding(RenderTreeBuilder builder, ref int sequence, int lastIndex)
            => RenderPaddingCore(
                builder,
                ref sequence,
                lastIndex + 1,
                Source.Count - 1,
                Math.Max(0, (Source.Count - lastIndex - 1) * ItemHeight));

        #endregion BuildRenderTree
    }

    public class ScrollableTableTheme
    {
        public string HeaderHeight { get; set; }

        public Dictionary<string, object> ElementAttributes { get; set; }
        public string ElementClass { get; set; }

        public string HeaderBackgroundClass { get; set; }
        public string HeaderBackgroundBackground { get; set; }
        public Dictionary<string, object> HeaderBackgroundAttributes { get; set; }

        public string ScrollerClass { get; set; }

        public Dictionary<string, object> ScrollerAttributes { get; set; }

        public string TableClass { get; set; }

        public Dictionary<string, object> TableAttributes { get; set; }

        public string TableHeadClass { get; set; }

        public Dictionary<string, object> TableHeadAttributes { get; set; }

        public string TableBodyClass { get; set; }
        public Dictionary<string, object> TableBodyAttributes { get; set; }

        public IDictionary<string, object> TableHeaderCellAttributes { get; set; }

        public IDictionary<string, object> TableHeaderCellInnerAttributes { get; set; }
    }

    public sealed class ItemTemplateContext<T>
          where T : class
    {
        public ItemTemplateContext(int index, T item)
        {
            Index = index;
            Item = item;
        }

        public int Index { get; }
        public T Item { get; }

        public override bool Equals(object obj)
            => obj is ItemTemplateContext<T> other
            && other.Index == Index
            && other.Item == Item;

        public override string ToString()
            => Index + ": " + (Item?.ToString() ?? "{null}");

        public override int GetHashCode()
        {
            var h = Item?.GetHashCode() ?? 0;

            return Index ^ (h >> 16) ^ (h << 16);
        }
    }

    public abstract class ItemsControl<T> : ListComponentBase<T>, IDisposable, IScrollEventListener
          where T : class
    {
        private const string ITEM_INDEX_ATTRIBUTE = "data-itemindex";
        private const string ITEM_LAST_INDEX_ATTRIBUTE = "data-itemlastindex";

        public static string ItemIndexAttribute => ITEM_INDEX_ATTRIBUTE;
        public static string ItemLastIndexAttribute => ITEM_LAST_INDEX_ATTRIBUTE;

        [Inject]
        public IJSRuntime JS { get; set; }

        [Parameter]
        public string ItemSelector { get; set; } = ":scope > *[data-itemindex]";

        #region ItemTemplate

        private RenderFragment<ItemTemplateContext<T>> _ItemTemplate;

        [Parameter]
        public RenderFragment<ItemTemplateContext<T>> ItemTemplate
        {
            get => _ItemTemplate;
            set => SetProperty(ref _ItemTemplate, value);
        }

        #endregion ItemTemplate

        protected ElementReference Element { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move && e.NewItems != null)
            {
                if (FirstIndex > 0
                    && e.NewStartingIndex + e.NewItems.Count < FirstIndex
                    && e.OldStartingIndex + e.NewItems.Count < FirstIndex)
                {
                    return;
                }
                if (LastIndex + 1 < Source?.Count
                    && e.NewStartingIndex + e.NewItems.Count > LastIndex
                    && e.OldStartingIndex + e.NewItems.Count > LastIndex)
                {
                    return;
                }
            }
            base.OnCollectionChanged(e);
        }

        protected override void OnItemAdded(T item)
        {
            base.OnItemAdded(item);
            CollectionChanged = true;
        }

        protected override void OnItemRemoved(T item)
        {
            base.OnItemRemoved(item);
            CollectionChanged = true;
        }

        protected override void OnReset()
        {
            base.OnReset();
            CollectionChanged = true;
        }

        protected override bool OnItemPropertyChanged(T item, string propertyName)
        {
            if (Source is IList list)
            {
                if (FirstIndex > 0 || LastIndex + 1 < Source.Count)
                {
                    var i = list.IndexOf(item);
                    if (i < FirstIndex || LastIndex < i)
                    {
                        return false;
                    }
                }
            }
            return base.OnItemPropertyChanged(item, propertyName);
        }

        #region CollectionChanged

        private bool _CollectionChanged;

        protected bool CollectionChanged
        {
            get => _CollectionChanged;
            private set => SetProperty(ref _CollectionChanged, value);
        }

        #endregion CollectionChanged

        #region Range

        private int _FirstIndex = -1;
        private int _LastIndex = -1;

        [Parameter]
        public int FirstIndex
        {
            get => _FirstIndex;
            set
            {
                var v = Math.Min(Math.Max(value, 0), (Source?.Count ?? 0) - 1);
                if (value != _FirstIndex
                    && Source?.Count > 0
                    && _LastIndex > _FirstIndex)
                {
                    _LastIndex += v - _FirstIndex;
                    _FirstIndex = v;

                    FirstIndexChanged?.Invoke(_FirstIndex);
                    LastIndexChanged?.Invoke(_LastIndex);

                    CollectionChanged = true;
                    StateHasChanged();
                }
            }
        }

        [Parameter]
        public Action<int> FirstIndexChanged { get; set; }

        [Parameter]
        public int LastIndex
        {
            get => _LastIndex;
            set
            {
                var v = Math.Min(Math.Max(value, 0), (Source?.Count ?? 0) - 1);
                if (v != _LastIndex
                    && Source?.Count > 0
                    && _LastIndex > _FirstIndex)
                {
                    _FirstIndex += v - _LastIndex;
                    _LastIndex = v;

                    FirstIndexChanged?.Invoke(_FirstIndex);
                    LastIndexChanged?.Invoke(_LastIndex);

                    CollectionChanged = true;

                    StateHasChanged();
                }
            }
        }

        [Parameter]
        public Action<int> LastIndexChanged { get; set; }

        protected abstract int ColumnCount { get; }

        protected async void SetVisibleRange(int first, int last, float localY, bool forceScroll)
        {
            first = Math.Max(0, first);
            last = Math.Min(last, Source?.Count ?? 0 - 1);
            if (last < 0 || first > last)
            {
                first = last = -1;
            }
            if (FirstIndex != first || LastIndex != last)
            {
                _FirstIndex = first;
                _LastIndex = last;

                FirstIndexChanged?.Invoke(_FirstIndex);
                LastIndexChanged?.Invoke(_LastIndex);

                StateHasChanged();
                if (forceScroll && first >= 0)
                {
                    await ScrollAsync(first, localY).ConfigureAwait(false);
                }
            }
            else if (forceScroll)
            {
                await ScrollAsync(first, localY).ConfigureAwait(false);
            }
        }

        #endregion Range

        #region MinimumRenderingCount

        private int _MinimumRenderingCount;

        [Parameter]
        public int MinimumRenderingCount
        {
            get => _MinimumRenderingCount;
            set => SetProperty(ref _MinimumRenderingCount, Math.Max(0, value));
        }

        #endregion MinimumRenderingCount

        #region IsVirtualized

        private bool _IsVirtualized = true;
        private bool _ShouldDetach;

        [Parameter]
        public bool IsVirtualized
        {
            get => _IsVirtualized;
            set
            {
                if (SetProperty(ref _IsVirtualized, value))
                {
                    _ShouldDetach = true;
                }
            }
        }

        #endregion IsVirtualized

        protected abstract void SetControlInfo(ItemsControlScrollInfo info, bool forceScroll, int? firstIndex = null);

        protected abstract void UpdateRange(ScrollInfo info, int firstIndex, float localY, bool forceScroll);

        protected virtual ValueTask ScrollAsync(int firstIndex, float localY)
            => JS.scrollToItem(Element, ItemSelector, firstIndex, localY, ColumnCount, false);

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if ((firstRender || _ShouldDetach) && _IsVirtualized)
            {
                _ShouldDetach = false;
                await JS.AttachWindowResize(this, Element).ConfigureAwait(false);
                await JS.AttachElementScroll(this, Element, ItemSelector).ConfigureAwait(false);
            }
            else if (!firstRender && _ShouldDetach && !_IsVirtualized)
            {
                _ShouldDetach = false;
                await JS.DetachWindowResize(this, Element).ConfigureAwait(false);
                await JS.DetachElementScroll(Element).ConfigureAwait(false);
            }

            if (CollectionChanged && _IsVirtualized)
            {
                _CollectionChanged = false;
                var si = await JS.GetItemsControlScrollInfoAsync(Element, ItemSelector).ConfigureAwait(false);
                SetLines(si.Lines);
                SetControlInfo(si, true, FirstIndex);
            }
        }

        [JSInvokable]
        public async void OnWindowResized()
        {
            if (IsVirtualized)
            {
                try
                {
                    var si = await JS.GetScrollInfoAsync(Element).ConfigureAwait(false);

                    UpdateRange(si, Math.Max(FirstIndex, 0), 0, true);
                }
                catch { }
            }
        }

        [JSInvokable]
        public void OnElementScroll(string jsonScrollInfo)
        {
            var si = JsonSerializer.Deserialize<ItemsControlScrollInfo>(jsonScrollInfo);
            SetLines(si.Lines);
            SetControlInfo(si, false);
        }

        protected List<ItemsControllLineInfo> Lines { get; } = new List<ItemsControllLineInfo>();

        private void SetLines(IList<ItemsControllLineInfo> lines)
        {
            Lines.Clear();
            if (lines != null)
            {
                Lines.AddRange(lines);
            }
        }

        #region BuildRenderTree

        protected virtual string GetTagName() => "div";

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var sequence = 0;
            builder.OpenElement(sequence++, GetTagName());

            builder.AddMultipleAttributes(
                sequence++,
                RuntimeHelpers.TypeCheck<IEnumerable<KeyValuePair<string, object>>>(AdditionalAttributes));

            builder.AddElementReferenceCapture(sequence++, v => Element = v);

            sequence = RenderItemsAndPaddings(builder, ref sequence);

            builder.CloseElement();
        }

        protected virtual int RenderItemsAndPaddings(RenderTreeBuilder builder, ref int sequence)
        {
            if (Source != null)
            {
                if (IsVirtualized)
                {
                    var firstIndex = FirstIndex;
                    var lastIndex = LastIndex;

                    GetRenderingRange(ref firstIndex, ref lastIndex);

                    RenderFirstPadding(builder, ref sequence, firstIndex);

                    if (firstIndex >= 0)
                    {
                        var li = Math.Min(lastIndex, Source.Count - 1);
                        for (var i = firstIndex; i <= li; i++)
                        {
                            builder.AddContent(sequence, ItemTemplate(new ItemTemplateContext<T>(i, Source[i])));
                        }
                    }

                    sequence++;

                    RenderLastPadding(builder, ref sequence, lastIndex);
                }
                else
                {
                    for (var i = 0; i < Source.Count; i++)
                    {
                        builder.AddContent(sequence, ItemTemplate(new ItemTemplateContext<T>(i, Source[i])));
                    }
                }
            }

            return sequence;
        }

        protected virtual void GetRenderingRange(ref int firstIndex, ref int lastIndex)
        {
            var rc = MinimumRenderingCount;
            if (Source?.Count > 0
                && rc > 0
                && (lastIndex - firstIndex + 1) < rc)
            {
                if (Source.Count <= rc)
                {
                    firstIndex = 0;
                    lastIndex = Source.Count - 1;
                }
                else if (0 <= firstIndex && firstIndex <= lastIndex)
                {
                    var bh = rc >> 1;
                    var ah = rc - bh;

                    var center = (firstIndex + lastIndex) >> 1;

                    if (center <= bh)
                    {
                        firstIndex = 0;
                        lastIndex = rc - 1;
                    }
                    else if (center + ah >= Source.Count - 1)
                    {
                        lastIndex = Source.Count - 1;
                        firstIndex = lastIndex - rc + 1;
                    }
                    else
                    {
                        firstIndex = center - bh;
                        lastIndex = center + ah;
                    }
                }
            }
        }

        protected static void RenderPaddingCore(RenderTreeBuilder builder, ref int sequence, int firstIndex, int lastIndex, float height, string tagName = "div")
        {
            builder.OpenElement(sequence++, tagName);
            if (height > 0)
            {
                builder.AddAttribute(sequence++, ITEM_INDEX_ATTRIBUTE, firstIndex);
                builder.AddAttribute(sequence++, ITEM_LAST_INDEX_ATTRIBUTE, lastIndex);
            }
            else
            {
                sequence += 2;
            }
            builder.AddAttribute(sequence++, "style", "margin:0;padding:0;width:100%;" + " height:" + Math.Max(0, height) + "px;opacity:0;overflow-anchor: none;");
            builder.CloseElement();
        }

        protected abstract void RenderFirstPadding(RenderTreeBuilder builder, ref int sequence, int firstIndex);

        protected abstract void RenderLastPadding(RenderTreeBuilder builder, ref int sequence, int lastIndex);

        #endregion BuildRenderTree

        #region IDisposable

        protected bool IsDisposed { get; set; }

        public void Dispose()
            => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing
                    && JS != null
                    && Element.Id != null
                    && (IsVirtualized || _ShouldDetach))
                {
                    JS.DetachWindowResize(this, Element);
                }
            }
            IsDisposed = true;
        }

        #endregion IDisposable
    }

    internal static class AttributeHelper
    {
        public static IEnumerable<KeyValuePair<string, object>> MergeAttributes(this IEnumerable<KeyValuePair<string, object>> element, IEnumerable<KeyValuePair<string, object>> theme)
        {
            if (element != null)
            {
                if (theme != null)
                {
                    return element.Concat(theme).GroupBy(e => e.Key).Select(e => e.First());
                }
                else
                {
                    return element;
                }
            }
            return theme ?? Enumerable.Empty<KeyValuePair<string, object>>();
        }

        public static IEnumerable<KeyValuePair<string, object>> AppendClass(this IEnumerable<KeyValuePair<string, object>> attributes, string cssClass)
        {
            if (attributes != null)
            {
                if (!string.IsNullOrEmpty(cssClass))
                {
                    return AppendClassCore(attributes, cssClass);
                }
                return attributes;
            }
            else if (!string.IsNullOrEmpty(cssClass))
            {
                return new[] { new KeyValuePair<string, object>("class", cssClass) };
            }
            return Enumerable.Empty<KeyValuePair<string, object>>(); ;
        }

        private static IEnumerable<KeyValuePair<string, object>> AppendClassCore(IEnumerable<KeyValuePair<string, object>> attributes, string cssClass)
        {
            var found = false;
            foreach (var kv in attributes)
            {
                if (!found && "class".Equals(kv.Key, StringComparison.InvariantCultureIgnoreCase))
                {
                    yield return new KeyValuePair<string, object>("class", kv.Value + " " + cssClass);
                    found = true;
                }
                else
                {
                    yield return kv;
                }
            }
            if (!found)
            {
                yield return new KeyValuePair<string, object>("class", cssClass);
            }
        }

        public static IEnumerable<KeyValuePair<string, object>> PrependStyle(this IEnumerable<KeyValuePair<string, object>> attributes, string style)
        {
            if (attributes != null)
            {
                if (!string.IsNullOrEmpty(style))
                {
                    return PrependStyleCore(attributes, style);
                }
                return attributes;
            }
            else if (!string.IsNullOrEmpty(style))
            {
                return new[] { new KeyValuePair<string, object>("style", style) };
            }
            return Enumerable.Empty<KeyValuePair<string, object>>(); ;
        }

        private static IEnumerable<KeyValuePair<string, object>> PrependStyleCore(IEnumerable<KeyValuePair<string, object>> s, string c)
        {
            var found = false;
            foreach (var kv in s)
            {
                if (!found && "style".Equals(kv.Key, StringComparison.InvariantCultureIgnoreCase))
                {
                    yield return new KeyValuePair<string, object>("style", c + ";" + kv.Value);
                    found = true;
                }
                else
                {
                    yield return kv;
                }
            }
            if (!found)
            {
                yield return new KeyValuePair<string, object>("style", c);
            }
        }
    }

    public abstract partial class BindableComponentBase : ComponentBase, ComponentUpdateScope.IComponent
    {
        #region BindableComponentBase

        private List<WeakReference<ComponentUpdateScope>> _Scopes;
        private bool _IsChangeDefered;

        protected ComponentUpdateScope BeginUpdate()
        {
            var s = new ComponentUpdateScope(this);
            (_Scopes ?? (_Scopes = new List<WeakReference<ComponentUpdateScope>>(4))).Add(new WeakReference<ComponentUpdateScope>(s));
            return s;
        }

        void ComponentUpdateScope.IComponent.EndUpdate(ComponentUpdateScope scope)
        {
            if (!ComponentUpdateScope.HasScopes(_Scopes, scope) && _IsChangeDefered)
            {
                _IsChangeDefered = false;
                base.StateHasChanged();
            }
        }

        protected new void StateHasChanged()
        {
            if (ComponentUpdateScope.HasScopes(_Scopes))
            {
                _IsChangeDefered = true;
            }
            else
            {
                _IsChangeDefered = false;
                base.StateHasChanged();
            }
        }

        #endregion BindableComponentBase
    }

    public abstract partial class BindableComponentBase<T> : BindableComponentBase, IBindableComponent
        where T : class
    {
        object IBindableComponent.DataContext => DataContext;
    }
}
