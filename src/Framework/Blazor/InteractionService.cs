using System.Text.Json;
using Shipwreck.BlazorJqueryToast;
using Shipwreck.ViewModelUtils.Components;
using Shipwreck.ViewModelUtils.Searching;

namespace Shipwreck.ViewModelUtils;

public class InteractionService : IInteractionService
{
    public InteractionService()
    {
        ModalOpeners = new Dictionary<Type, Type>()
        {
            [typeof(SearchPropertiesModalViewModel)] = typeof(SearchPropertiesModal)
        };
    }

    #region InvokeAsync

    bool IInteractionService.InvokeRequired => false;

    Task IInteractionService.InvokeAsync(object context, Action action)
    {
        if (context is IHasBindableComponent bc && bc.Component != null)
        {
            return bc.Component.InvokeAsync(action);
        }
        return Task.Run(action);
    }

    Task<T> IInteractionService.InvokeAsync<T>(object context, Func<T> func)
    {
        if (context is IHasBindableComponent bc && bc.Component != null)
        {
            T result = default;
            return bc.Component.InvokeAsync(() => result = func()).ContinueWith(t => result);
        }

        return Task.Run(func);
    }

    #endregion InvokeAsync

    #region トースト

    bool IInteractionService.SupportsToasts => true;

    public Task ShowErrorToastAsync(object context, string message, string title)
        => ShowToastAsync(context, BorderStyle.Danger, ToastIcon.Error, message, title);

    public Task ShowInformationToastAsync(object context, string message, string title)
        => ShowToastAsync(context, BorderStyle.Info, ToastIcon.Information, message, title);

    public Task ShowSuccessToastAsync(object context, string message, string title)
        => ShowToastAsync(context, BorderStyle.Success, ToastIcon.Success, message, title);

    public Task ShowWarningToastAsync(object context, string message, string title)
        => ShowToastAsync(context, BorderStyle.Warning, ToastIcon.Warning, message, title);

    protected virtual Task ShowToastAsync(object context, BorderStyle style, ToastIcon icon, string message, string title)
    {
        try
        {
            var page = (context as IHasFrameworkPageViewModel)?.Page;

            if (page != null)
            {
                page.EnqueueToastLog(style, message, title);

                if (page.OverridesToast(message, title, style))
                {
                    return Task.CompletedTask;
                }

                if (!SupportsJSToast(page))
                {
                    page.LogWarning("Toast is not supported in Blazor Server. {{ title: {0}, message: {1}, style: {2} }}", title, message, style);
                    return Task.CompletedTask;
                }
            }

            var tp = (context as IHasToastPresenter)?.ToastPresenter;
            if (tp != null)
            {
                tp.Add(style, message, title);
                return Task.CompletedTask;
            }

            var js = (context as IHasJSRuntime)?.JS;

            return js?.ShowToastAsync(new ToastOptions
            {
                Text = message,
                Icon = icon,
                Position = ToastPosition.BottomCenter,
                ShowLoader = false,
            }).AsTask() ?? Task.CompletedTask;
        }
        catch
        {
        }
        return default;
    }

    protected virtual bool SupportsJSToast(FrameworkPageViewModel page)
        => page.IsWebAssembly;

    #endregion トースト

    #region メッセージボックス

    bool IInteractionService.SupportsMessageBoxes
        => true;

    public virtual Task AlertAsync(object context, string message, string title, string buttonText, BorderStyle? buttonStyle)
        => ShowErrorToastAsync(context, message, title);

    public virtual Task<bool> ConfirmAsync(object context,
            string message,
            string title = null,
            string trueText = null,
            BorderStyle? trueStyle = null,
            string falseText = null,
            BorderStyle? falseStyle = null)
        => ConfirmModal.ShowAsync(
            (context as IHasInteractionService)?.Interaction,
            (context as IHasModalPresenter)?.ModalPresenter,
            message: message,
            title: title,
            trueText: trueText,
            trueStyle: trueStyle,
            falseText: falseText,
            falseStyle: falseStyle);

    #endregion メッセージボックス

    #region ファイル

    bool IInteractionService.SupportsFileDialogs => false;

    public Task<string> OpenDirectoryAsync(object context, string directoryName = null)
        => Task.FromResult<string>(null);

    public Task<string[]> OpenFilesAsync(object context, string filter, int filterIndex = 0, string fileName = null, string initialDirectory = null, bool multiSelect = false)
        => Task.FromResult(Array.Empty<string>());

    public Task<string> SaveDirectoryAsync(object context, string directoryName = null)
        => Task.FromResult<string>(null);

    public Task<string> SaveFileAsync(object context, string filter, int filterIndex = 0, string fileName = null, string initialDirectory = null)
        => Task.FromResult<string>(null);

    #endregion ファイル

    #region モーダル

    protected readonly Dictionary<Type, Type> ModalOpeners;

    public InteractionService RegisterModal(Type viewModelType, Type viewType)
    {
        if (viewType == null)
        {
            ModalOpeners.Remove(viewModelType);
        }
        else
        {
            ModalOpeners[viewModelType] = viewType;
        }
        return this;
    }

    public virtual bool IsModalSupported(object context, Type viewModelType)
        => ModalOpeners.ContainsKey(viewModelType)
        || (viewModelType.BaseType != null && IsModalSupported(context, viewModelType.BaseType));

    public virtual Task OpenModalAsync(object context, object viewModel)
    {
        try
        {
            var mp = (viewModel as IHasModalPresenter)?.ModalPresenter
                ?? (context as IHasModalPresenter)?.ModalPresenter
                ?? ((context as IHasFrameworkPageViewModel)?.Page as IHasModalPresenter)?.ModalPresenter;

            if (mp != null)
            {
                for (var t = viewModel?.GetType(); t != null; t = t.BaseType)
                {
                    if (ModalOpeners.TryGetValue(t, out var f))
                    {
                        mp.ShowModal(f, new Dictionary<string, object>
                        {
                            [nameof(ModalBase<object>.DataContext)] = viewModel,
                            [nameof(ModalBase<object>.IsOpen)] = true,
                        });

                        return Task.CompletedTask;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);
        }
        return Task.FromException(new NotSupportedException());
    }

    public virtual Task CloseModalAsync(object context, object viewModel)
    {
        var mp = (viewModel as IHasModalPresenter)?.ModalPresenter
            ?? (context as IHasModalPresenter)?.ModalPresenter;

        mp?.CloseModal();

        return Task.CompletedTask;
    }

    #endregion モーダル

    #region DownloadAsync

    bool IInteractionService.SupportsDownload => true;

    public Task DownloadAsync(object context, string method, string url, string content, string contentType, bool openFile)
        => DownloadAsync(context, method, url, null, content, contentType, openFile);

    public virtual Task DownloadAsync(object context, string method, string url, Dictionary<string, string> headers, string content, string contentType, bool openFile)
    {
        var js = (context as IHasJSRuntime)?.JS;

        return js.InvokeVoidAsync(
            "Shipwreck.ViewModelUtils.downloadFile",
            DotNetObjectReference.Create(this),
            method,
            url,
            headers?.Count > 0 ? JsonSerializer.Serialize(headers) : "{}",
            content,
            contentType,
            openFile).AsTask();
    }

    [JSInvokable]
    public virtual string GetDownloadingFileName(string requestUri, string responseUri, string headerJson)
    {
        var headers = string.IsNullOrEmpty(headerJson)
            ? null
            : JsonSerializer.Deserialize<Dictionary<string, string>>(headerJson);

        headers = headers?.Count > 0
            ? new Dictionary<string, string>(headers, StringComparer.InvariantCultureIgnoreCase)
            : new Dictionary<string, string>();

        if (headers.TryGetValue("content-disposition", out var contentDisposition)
            && !string.IsNullOrEmpty(contentDisposition)
            && System.Net.Http.Headers.ContentDispositionHeaderValue.TryParse(contentDisposition, out var v))
        {
            if (!string.IsNullOrEmpty(v.FileNameStar))
            {
                return Uri.UnescapeDataString(v.FileNameStar);
            }
            if (!string.IsNullOrEmpty(v.FileName))
            {
                return Uri.UnescapeDataString(v.FileName);
            }
        }
        if ((headers.TryGetValue("x-file-name", out var fn) && !string.IsNullOrEmpty(fn))
            || (headers.TryGetValue("x-filename", out fn) && !string.IsNullOrEmpty(fn)))
        {
            return Uri.UnescapeDataString(fn);
        }

        foreach (var us in new[] { responseUri, requestUri })
        {
            if (Uri.TryCreate(us, UriKind.Absolute, out var u)
                && Regex.Match(u.AbsolutePath, @"[^/]+\.[^./]+") is var m
                && m.Success)
            {
                return Uri.UnescapeDataString(m.Value);
            }
        }

        var lastComp = "download";

        if (headers.TryGetValue("content-type", out var ct))
        {
            switch (ct)
            {
                case "image/jpeg":
                    return lastComp + ".jpg";

                case "image/png":
                    return lastComp + ".png";

                case "text/csv":
                    return lastComp + ".csv";

                case "application/vnd.ms-excel":
                    return lastComp + ".xls";

                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return lastComp + ".xlsx";
            }
        }

        return lastComp;
    }

    #endregion DownloadAsync
}
