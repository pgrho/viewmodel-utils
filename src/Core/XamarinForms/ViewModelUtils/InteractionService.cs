using System.IO;
using System.Net.Http;
using Xamarin.Essentials;

namespace Shipwreck.ViewModelUtils;

public abstract class InteractionService : IInteractionService
{
    private static string TrimOrNull(string s)
        => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    protected virtual Page ResolvePage(object context)
        => context as Page ?? (context as IHasPage)?.Page;

    #region InvokeAsync

    public bool InvokeRequired => !MainThread.IsMainThread;

    public Task InvokeAsync(object context, Action action)
        => Device.InvokeOnMainThreadAsync(action);

    public Task<T> InvokeAsync<T>(object context, Func<T> func)
         => Device.InvokeOnMainThreadAsync(func);

    #endregion InvokeAsync

    #region Toasts

    public virtual bool SupportsToasts => true;

    public abstract Task ShowSuccessToastAsync(object context, string message, string title);

    public abstract Task ShowErrorToastAsync(object context, string message, string title);

    public abstract Task ShowWarningToastAsync(object context, string message, string title);

    public abstract Task ShowInformationToastAsync(object context, string message, string title);

    #endregion Toasts

    #region Download

    public bool SupportsDownload => true;

    public Task DownloadAsync(
        object context,
        string method,
        string url,
        string content,
        string contentType,
        bool openFile)
        => DownloadAsync(context, method, url, content, contentType, openFile, null);

    public virtual async Task DownloadAsync(
        object context,
        string method,
        string url,
        string content,
        string contentType,
        bool openFile,
        Action<string> opener)
    {
        // var page = context as PageViewModel ?? (context as IHasPageViewModel)?.Page;

        try
        {
            var res = await SendAsync(context, method, url, content, contentType);

            if (!res.IsSuccessStatusCode)
            {
                if (SupportsToasts)
                {
                    await ShowErrorToastAsync(context, "ファイルの取得に失敗しました。", null);
                }
                return;
            }

            var ou = (res.Headers.Location ?? res.RequestMessage.RequestUri).AbsolutePath;

            var fn = TrimOrNull(res.Content.Headers.ContentDisposition?.FileNameStar)
                    ?? TrimOrNull(res.Content.Headers.ContentDisposition?.FileName)
                    ?? (ou?.LastOrDefault() != '/' ? Path.GetFileName(ou) : "download");

            var file = Path.Combine(FileSystem.CacheDirectory, fn);
            using (var s = await res.Content.ReadAsStreamAsync())
            using (var d = new FileStream(file, FileMode.Create))
            {
                await s.CopyToAsync(d);
            }

            Console.WriteLine("ファイルを出力しました: {0}", file);

            if (opener != null)
            {
                opener(file);
                return;
            }

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = fn,
                File = new ShareFile(file)
            });
        }
        catch (Exception ex)
        {
            if (SupportsToasts)
            {
                await ShowErrorToastAsync(context, "ファイルの取得に失敗しました。", null);
            }
            Console.WriteLine("ファイルの取得に失敗しました。{0}", ex);
        }
    }

    protected virtual async Task<HttpResponseMessage> SendAsync(object context,
        string method,
        string url,
        string content,
        string contentType)
    {
        using (var c = new HttpClient())
        {
            var req = new HttpRequestMessage(new HttpMethod(method), url);

            if (!string.IsNullOrEmpty(content))
            {
                req.Content = new StringContent(content, Encoding.UTF8, contentType);
            }

            return await c.SendAsync(req);
        }
    }

    #endregion Download

    #region MessageBox

    public virtual bool SupportsMessageBoxes => true;

    public virtual Task AlertAsync(object context, string message, string title, string buttonText, BorderStyle? buttonStyle)
        => ResolvePage(context)?.DisplayAlert(title ?? string.Empty, message, buttonText ?? "OK")
            ?? Task.FromException(new NotSupportedException());

    public virtual Task<bool> ConfirmAsync(object context, string message, string title, string trueText, BorderStyle? trueStyle, string falseText, BorderStyle? falseStyle)
        => ResolvePage(context)?.DisplayAlert(title ?? string.Empty, message, trueText ?? "OK", falseText ?? "キャンセル")
            ?? Task.FromException<bool>(new NotSupportedException());

    #endregion MessageBox

    #region FileDialog

    public virtual bool SupportsFileDialogs => false;

    public virtual Task<string[]> OpenFilesAsync(object context, string filter, int filterIndex = 0, string fileName = null, string initialDirectory = null, bool multiSelect = false) => throw new NotImplementedException();

    public virtual Task<string> SaveFileAsync(object context, string filter, int filterIndex = 0, string fileName = null, string initialDirectory = null) => throw new NotImplementedException();

    public virtual Task<string> OpenDirectoryAsync(object context, string directoryName = null) => throw new NotImplementedException();

    public virtual Task<string> SaveDirectoryAsync(object context, string directoryName = null) => throw new NotImplementedException();

    #endregion FileDialog

    #region モーダル

    private readonly Dictionary<Type, Func<object, Page>> _ModalCreators = new Dictionary<Type, Func<object, Page>>();

    private ConditionalWeakTable<object, TaskCompletionSource<object>> _ModalTasks = new ConditionalWeakTable<object, TaskCompletionSource<object>>();

    public virtual bool IsModalSupported(object context, Type viewModelType)
        => _ModalCreators.ContainsKey(viewModelType)
        || (viewModelType.BaseType != null && IsModalSupported(context, viewModelType.BaseType));

    public virtual Task OpenModalAsync(object context, object viewModel)
    {
        if (context is IHasPage hp
            && hp.Page is IHasPseudoModal hpm
            && hpm.TryOpen(viewModel))
        {
            return Task.CompletedTask;
        }

        var nav = Application.Current?.MainPage.Navigation;
        if (nav != null)
        {
            for (var t = viewModel?.GetType(); t != null; t = t.BaseType)
            {
                if (_ModalCreators.TryGetValue(t, out var f))
                {
                    var p = f(viewModel);
                    if (p != null)
                    {
                        DependencyService.Get<IKeyboardService>()?.Hide();
                        p.Disappearing -= P_Disappearing;
                        p.Disappearing += P_Disappearing;

                        nav.PushModalAsync(p);

                        return _ModalTasks.GetValue(viewModel, _ => new TaskCompletionSource<object>()).Task;
                    }
                }
            }
        }
        return Task.FromException(new NotSupportedException());
    }

    private void P_Disappearing(object sender, EventArgs e)
    {
        if (sender is Page p && p.Navigation?.ModalStack.Contains(p) != true)
        {
            p.Disappearing -= P_Disappearing;
            if (p.BindingContext != null)
            {
                _ModalTasks.TryGetValue(p.BindingContext, out var tcs);
                tcs?.TrySetResult(null);
            }
        }
    }

    public virtual Task CloseModalAsync(object context, object viewModel)
    {
        if (context is IHasPage hp
            && hp.Page is IHasPseudoModal hpm
            && hpm.TryClose(viewModel))
        {
            return Task.CompletedTask;
        }

        var p = ResolvePage(context);
        var fm = p?.Navigation?.ModalStack?.LastOrDefault();
        if (fm?.BindingContext == viewModel)
        {
            return p.Navigation.PopModalAsync();
        }
        return Task.CompletedTask;
    }

    public InteractionService RegisterModal(Type viewModelType, Func<object, Page> creator)
    {
        if (creator == null)
        {
            _ModalCreators.Remove(viewModelType);
        }
        else
        {
            _ModalCreators[viewModelType] = creator;
        }
        return this;
    }

    #endregion モーダル
}
