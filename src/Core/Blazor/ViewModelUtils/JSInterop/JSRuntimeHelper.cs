using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Shipwreck.ViewModelUtils.JSInterop
{
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
}
