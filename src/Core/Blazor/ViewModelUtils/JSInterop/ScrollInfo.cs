using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace Shipwreck.ViewModelUtils.JSInterop
{
    public interface IScrollEventListener : IWindowResizeEventListener
    {
        void OnElementScroll(string jsonScrollInfo);
    }
    public sealed class ItemsControllElementInfo
    {
        public int FirstIndex { get; set; }
        public int LastIndex { get; set; }
        public float Left { get; set; }
        public float Top { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public float Right => Left + Width;
        public float Bottom => Top + Height;

        public override string ToString()
            => $"{FirstIndex}-{LastIndex} {{{Left}-{Right}, {Top}-{Bottom}}}";
    }
    public sealed class ItemsControllLineInfo
    {
        public int FirstIndex { get; set; }
        public int LastIndex { get; set; }
        public float Top { get; set; }
        public float Height { get; set; }

        public float Bottom => Top + Height;

        public override string ToString()
            => $"{FirstIndex}-{LastIndex} {{{Top}-{Bottom}}}";
    }
    public sealed class ItemsControlScrollInfo
    {
        public ScrollInfo Viewport { get; set; }
        public ItemsControllElementInfo First { get; set; }
        public ItemsControllElementInfo Last { get; set; }
        public float MinWidth { get; set; }
        public float MinHeight { get; set; }

        public IList<ItemsControllLineInfo> Lines { get; set; }
    }
    public interface IWindowResizeEventListener
    {
        void OnWindowResized();
    }
    public static class JSRuntimeHelper
    {
        public static ValueTask AttachWindowResize(this IJSRuntime js, IWindowResizeEventListener listener, ElementReference element)
            => js.InvokeVoidAsync(
                "Shipwreck.ViewModelUtils.attachWindowResize",
                DotNetObjectReference.Create(listener),
                element);

        public static ValueTask ScrollTo(this IJSRuntime js, ElementReference element, float left, float top, bool isSmooth)
            => js.InvokeVoidAsync(
                "Shipwreck.ViewModelUtils.scrollTo", element, left, top, isSmooth);

        public static ValueTask scrollToItem(this IJSRuntime js, ElementReference element, string itemSelector, int index, float localY, int column, bool isSmooth)
            => js.InvokeVoidAsync(
                "Shipwreck.ViewModelUtils.scrollToItem", element, itemSelector, index, localY, column, isSmooth);

        public static ValueTask DetachWindowResize(this IJSRuntime js, IWindowResizeEventListener listener, ElementReference element)
            => js.InvokeVoidAsync(
                "Shipwreck.ViewModelUtils.detachWindowResize",
                DotNetObjectReference.Create(listener),
                element);

        public static ValueTask AttachElementScroll(this IJSRuntime js, IScrollEventListener listener, ElementReference element, string itemSelector)
            => js.InvokeVoidAsync(
                "Shipwreck.ViewModelUtils.attachElementScroll",
                element,
                DotNetObjectReference.Create(listener),
                itemSelector);

        public static ValueTask DetachElementScroll(this IJSRuntime js, ElementReference element)
            => js.InvokeVoidAsync(
                "Shipwreck.ViewModelUtils.detachElementScroll",
                element);

        public static async ValueTask<ScrollInfo> GetScrollInfoAsync(this IJSRuntime js, ElementReference element)
        {
            var json = await js.InvokeAsync<string>("Shipwreck.ViewModelUtils.getScrollInfo", element).ConfigureAwait(false);

            return JsonSerializer.Deserialize<ScrollInfo>(json);
        }

        public static async ValueTask<ItemsControlScrollInfo> GetItemsControlScrollInfoAsync(this IJSRuntime js, ElementReference element, string itemSelector)
        {
            var json = await js.InvokeAsync<string>("Shipwreck.ViewModelUtils.getItemsControlScrollInfo", element, itemSelector).ConfigureAwait(false);

            return JsonSerializer.Deserialize<ItemsControlScrollInfo>(json);
        }
    }
    public class ScrollInfo
    {
        public float ClientLeft { get; set; }
        public float ClientTop { get; set; }
        public float ClientWidth { get; set; }
        public float ClientHeight { get; set; }
        public float ScrollLeft { get; set; }
        public float ScrollTop { get; set; }
        public float ScrollWidth { get; set; }
        public float ScrollHeight { get; set; }

        public float ClientRight => ClientLeft + ClientWidth;
        public float ClientBottom => ClientTop + ClientHeight;

        public override string ToString()
            => $"{{{ClientLeft}-{ClientRight}, {ClientTop}-{ClientBottom}}} {{{ScrollLeft}, {ScrollTop}}}";
    }
}
