using System.Text.Json;

namespace Shipwreck.ViewModelUtils.JSInterop;

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
        var json = await js.InvokeAsync<string>("Shipwreck.ViewModelUtils.getScrollInfo", element).ConfigureAwait();

        return JsonSerializer.Deserialize<ScrollInfo>(json);
    }

    public static async ValueTask<ItemsControlScrollInfo> GetItemsControlScrollInfoAsync(this IJSRuntime js, ElementReference element, string itemSelector)
    {
        var json = await js.InvokeAsync<string>("Shipwreck.ViewModelUtils.getItemsControlScrollInfo", element, itemSelector).ConfigureAwait();

        return JsonSerializer.Deserialize<ItemsControlScrollInfo>(json);
    }

    public static string GetUserAgent(this IJSRuntime js)
     => ((IJSInProcessRuntime)js).Invoke<string>("Shipwreck.ViewModelUtils.userAgent");

    #region localStorage

    #region string

    public static ValueTask<string> ReadLocalStorageAsync(this IJSRuntime js, string name)
        => js.InvokeAsync<string>("Shipwreck.ViewModelUtils.readLocalStorage", name);

    public static ValueTask WriteLocalStorageAsync(this IJSRuntime js, string name, string value)
        => js.InvokeVoidAsync("Shipwreck.ViewModelUtils.writeLocalStorage", name, value);

    public static ValueTask<string> ReadSessionStorageAsync(this IJSRuntime js, string name)
        => js.InvokeAsync<string>("Shipwreck.ViewModelUtils.readSessionStorage", name);

    public static ValueTask WriteSessionStorageAsync(this IJSRuntime js, string name, string value)
        => js.InvokeVoidAsync("Shipwreck.ViewModelUtils.writeSessionStorage", name, value);

    public static string ReadLocalStorage(this IJSInProcessRuntime js, string name)
        => js.Invoke<string>("Shipwreck.ViewModelUtils.readLocalStorage", name);

    public static void WriteLocalStorage(this IJSInProcessRuntime js, string name, string value)
        => js.InvokeVoid("Shipwreck.ViewModelUtils.writeLocalStorage", name, value);

    public static string ReadSessionStorage(this IJSInProcessRuntime js, string name)
        => js.Invoke<string>("Shipwreck.ViewModelUtils.readSessionStorage", name);

    public static void WriteSessionStorage(this IJSInProcessRuntime js, string name, string value)
        => js.InvokeVoid("Shipwreck.ViewModelUtils.writeSessionStorage", name, value);

    #endregion string

    #region Int32

    public static async ValueTask<int?> ReadLocalStorageAsInt32Async(this IJSRuntime js, string name)
        => int.TryParse(await js.ReadLocalStorageAsync(name).ConfigureAwait(), out var i) ? i : (int?)null;

    public static ValueTask WriteLocalStorageAsync(this IJSRuntime js, string name, int? value)
        => js.WriteLocalStorageAsync(name, value?.ToString() ?? string.Empty);

    public static async ValueTask<int?> ReadSessionStorageAsInt32Async(this IJSRuntime js, string name)
        => int.TryParse(await js.ReadSessionStorageAsync(name).ConfigureAwait(), out var i) ? i : (int?)null;

    public static ValueTask WriteSessionStorageAsync(this IJSRuntime js, string name, int? value)
        => js.WriteSessionStorageAsync(name, value?.ToString() ?? string.Empty);

    public static int? ReadLocalStorageAsInt32(this IJSInProcessRuntime js, string name)
        => int.TryParse(js.ReadLocalStorage(name), out var i) ? i : (int?)null;

    public static void WriteLocalStorage(this IJSInProcessRuntime js, string name, int? value)
        => js.WriteLocalStorage(name, value?.ToString() ?? string.Empty);

    public static int? ReadSessionStorageAsInt32(this IJSInProcessRuntime js, string name)
        => int.TryParse(js.ReadSessionStorage(name), out var i) ? i : (int?)null;

    public static void WriteSessionStorage(this IJSInProcessRuntime js, string name, int? value)
        => js.WriteSessionStorage(name, value?.ToString() ?? string.Empty);

    #endregion Int32

    #endregion localStorage

    public static async ValueTask<JsonHttpResponse> SendFileAsync(this IJSRuntime js, string method, string url, IEnumerable<KeyValuePair<string, string>> headers, ElementReference input)
    {
        var json = await js.InvokeAsync<string>(
            "Shipwreck.ViewModelUtils.sendFiles",
            method,
            url,
            headers != null ? JsonSerializer.Serialize(new Dictionary<string, string>(headers)) : null,
            input).ConfigureAwait();
        return JsonSerializer.Deserialize<JsonHttpResponse>(json);
    }

    public static ValueTask<bool> OpenWindowAsync(this IJSRuntime js, string url, string name, string features)
        => js.InvokeAsync<bool>(
            "Shipwreck.ViewModelUtils.openWindow",
            url, name, features);

    public static ValueTask FocusAsync(this IJSRuntime js, ElementReference element, bool selectAll)
        => js.InvokeVoidAsync("Shipwreck.ViewModelUtils.focus", element, selectAll);

    internal static ValueTask FocusAsyncWithWarning(this IJSRuntime js, ElementReference element, bool selectAll, string parentName)
    {
        if (element.Id == null)
        {
            Console.WriteLine("Warning! {0} passed default ElementReference to FocusAsync", parentName);
        }
        return js.FocusAsync(element, selectAll);
    }

    internal static ValueTask FocusAsyncWithWarning(this ElementReference element, string parentName)
    {
        if (element.Id == null)
        {
            Console.WriteLine("Warning! {0} passed default ElementReference to FocusAsync", parentName);
        }
        return element.FocusAsync();
    }
}
