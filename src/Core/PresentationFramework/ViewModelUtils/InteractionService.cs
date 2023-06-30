using System.Collections;
using System.IO;
using System.Net.Http;
using Microsoft.Win32;

namespace Shipwreck.ViewModelUtils;

public class InteractionService : IInteractionService
{
    protected virtual Window GetWindow(object context)
    {
        if (context is IHasWindow hw && hw.Window != null)
        {
            return hw.Window;
        }
        var app = Application.Current;
        if (app != null)
        {
            return (context != null ? app.Windows.Cast<Window>().FirstOrDefault(e => e.DataContext == context) : null)
                ?? app.Windows.OfType<Window>().FirstOrDefault(e => e.IsActive)
                ?? app.MainWindow;
        }
        return null;
    }

    #region InvokeAsync

    public bool InvokeRequired
        => Application.Current?.Dispatcher is var d && d.Thread != Thread.CurrentThread;

    public Task InvokeAsync(object context, Action operation)
        => InvokeAsync(context, () => { operation(); return 0; });

    public Task<T> InvokeAsync<T>(object context, Func<T> operation)
    {
        var dispatcher = Application.Current?.Dispatcher;

        if (dispatcher?.Thread == null
            || dispatcher.Thread == Thread.CurrentThread)
        {
            return Task.FromResult(operation());
        }
        else
        {
            var tcs = new TaskCompletionSource<T>();

            dispatcher.BeginInvoke((Action)(() =>
            {
                try
                {
                    tcs.TrySetResult(operation());
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
                finally
                {
                    tcs.TrySetCanceled();
                }
            }));

            return tcs.Task;
        }
    }

    public Task InvokeAsync(object context, Func<Task> operation)
    {
        var dispatcher = Application.Current?.Dispatcher;

        if (dispatcher?.Thread == null
            || dispatcher.Thread == Thread.CurrentThread)
        {
            return operation();
        }
        else
        {
            var tcs = new TaskCompletionSource<object>();

            dispatcher.BeginInvoke((Action)(async () =>
            {
                try
                {
                    await operation();
                    tcs.TrySetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
                finally
                {
                    tcs.TrySetCanceled();
                }
            }));

            return tcs.Task;
        }
    }

    #endregion InvokeAsync

    #region Toast

    bool IInteractionService.SupportsToasts => true;

    public Task ShowSuccessToastAsync(object context, string message, string title)
        => ShowToastAsync(context, message, title, BorderStyle.Success);

    public Task ShowErrorToastAsync(object context, string message, string title)
        => ShowToastAsync(context, message, title, BorderStyle.Danger);

    public Task ShowWarningToastAsync(object context, string message, string title)
        => ShowToastAsync(context, message, title, BorderStyle.Warning);

    public Task ShowInformationToastAsync(object context, string message, string title)
        => ShowToastAsync(context, message, title, BorderStyle.Info);

    public virtual Task ShowToastAsync(object context, string message, string title, BorderStyle style)
        => InvokeAsync(context, () => ShowToast(context, message, title, style));

    protected virtual void ShowToast(object context, string message, string title, BorderStyle style)
    {
        if (context is IHasFrameworkPageViewModel hp
            && hp.Page is FrameworkPageViewModel pvm)
        {
            pvm.EnqueueToastLog(style, message, title);

            if (pvm.OverridesToast(message, title, style))
            {
                return;
            }
        }

        MessageBox.Show(GetWindow(context), message, title);
    }

    #endregion Toast

    #region メッセージ

    bool IInteractionService.SupportsMessageBoxes => true;

    public Task AlertAsync(
        object context,
        string message,
        string title,
        string buttonText,
        BorderStyle? buttonStyle)
        => ShowMessageBoxAsync(
            context,
            message,
            title,
            buttonText,
            buttonStyle,
            null,
            default,
            MessageBoxButton.OK,
            MessageBoxImage.Warning);

    public Task<bool> ConfirmAsync(
        object context,
        string message,
        string title,
        string trueText,
        BorderStyle? trueStyle,
        string falseText,
        BorderStyle? falseStyle)
        => ShowMessageBoxAsync(
            context,
            message,
            title,
            trueText,
            trueStyle,
            falseText,
            falseStyle,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question).ContinueWith(t => t.Result == MessageBoxResult.Yes);

    public virtual Task<MessageBoxResult> ShowMessageBoxAsync(
        object context,
        string message,
        string title,
        string trueText,
        BorderStyle? trueStyle,
        string falseText,
        BorderStyle? falseStyle,
        MessageBoxButton button,
        MessageBoxImage icon)
        => InvokeAsync(
            context,
            () => ShowMessageBox(
                context,
                message,
                title,
                trueText,
                trueStyle,
                falseText,
                falseStyle,
                button,
                icon));

    protected virtual MessageBoxResult ShowMessageBox(
        object context,
        string message,
        string title,
        string trueText,
        BorderStyle? trueStyle,
        string falseText,
        BorderStyle? falseStyle,
        MessageBoxButton button,
        MessageBoxImage icon)
        => MessageBox.Show(GetWindow(context), message, title, button, icon);

    #endregion メッセージ

    #region ファイル

    bool IInteractionService.SupportsFileDialogs => true;

    public virtual Task<string[]> OpenFilesAsync(
        object context,
        string filter,
        int filterIndex = 0,
        string fileName = null,
        string initialDirectory = null,
        bool multiSelect = false)
        => InvokeAsync(context,
            () => OpenFiles(
                context,
                filter,
                filterIndex: filterIndex,
                fileName: fileName,
                initialDirectory: initialDirectory,
                multiSelect: multiSelect));

    protected virtual string[] OpenFiles(
        object context,
        string filter,
        int filterIndex = 0,
        string fileName = null,
        string initialDirectory = null,
        bool multiSelect = false)
    {
        var ofd = new OpenFileDialog()
        {
            Filter = filter,
            FilterIndex = filterIndex,
            FileName = fileName,
            InitialDirectory = initialDirectory,
            Multiselect = multiSelect
        };

        if (ofd.ShowDialog(GetWindow(context)) == true)
        {
            return ofd.FileNames;
        }
        return null;
    }

    public virtual Task<string> SaveFileAsync(
        object context,
        string filter,
        int filterIndex = 0,
        string fileName = null,
        string initialDirectory = null)
        => InvokeAsync(context,
            () => SaveFile(
                context,
                filter,
                filterIndex: filterIndex,
                fileName: fileName,
                initialDirectory: initialDirectory));

    protected virtual string SaveFile(
        object context,
        string filter,
        int filterIndex = 0,
        string fileName = null,
        string initialDirectory = null)
    {
        var d = new SaveFileDialog()
        {
            Filter = filter,
            FilterIndex = filterIndex,
            FileName = fileName,
            InitialDirectory = initialDirectory
        };

        if (d.ShowDialog(GetWindow(context)) == true)
        {
            return d.FileName;
        }
        return null;
    }

    public virtual Task<string> OpenDirectoryAsync(
        object context,
        string directoryName = null)
        => InvokeAsync(
            context,
            () => OpenDirectory(
                context,
                directoryName: directoryName));

    protected virtual string OpenDirectory(
        object context,
        string directoryName = null)
        => ShowDirectoryDialog(
            context,
            new OpenFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = false,
                ValidateNames = false,
                DereferenceLinks = true,
                InitialDirectory = directoryName,
                FileName = "ディレクトリ"
            });

    public virtual Task<string> SaveDirectoryAsync(
        object context,
        string directoryName = null)
        => InvokeAsync(
            context,
            () => SaveDirectory(
                context,
                directoryName: directoryName));

    protected virtual string SaveDirectory(
        object context,
        string directoryName = null)
        => ShowDirectoryDialog(
            context,
            new SaveFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = false,
                ValidateNames = false,
                DereferenceLinks = true,
                InitialDirectory = directoryName,
                OverwritePrompt = false,
                FileName = "ディレクトリ"
            });

    private string ShowDirectoryDialog(object context, FileDialog d)
    {
        if (d.ShowDialog(GetWindow(context)) == true)
        {
            var n = d.FileName;

            if (n?.Length > 0)
            {
                var dn = Path.GetDirectoryName(n);
                if (File.Exists(n) || Directory.Exists(dn))
                {
                    return dn;
                }

                return n;
            }
        }

        return null;
    }

    #endregion ファイル

    #region モーダル

    private readonly Dictionary<Type, Func<object, FrameworkElement>> _ModalCreators
        = new Dictionary<Type, Func<object, FrameworkElement>>();

    public void RegisterModal(Type viewModelType, Func<object, FrameworkElement> handler)
    {
        lock (((ICollection)_ModalCreators).SyncRoot)
        {
            _ModalCreators[viewModelType ?? throw new ArgumentNullException(nameof(viewModelType))]
                = handler ?? throw new ArgumentNullException(nameof(handler));
        }
    }

    protected static Func<object, T> GetCreator<T>(Type viewModelType, Dictionary<Type, Func<object, T>> creators)
    {
        lock (((ICollection)creators).SyncRoot)
        {
            for (var t = viewModelType; t != null; t = t.BaseType)
            {
                if (creators.TryGetValue(t, out var f))
                {
                    return f;
                }
            }
        }
        return null;
    }

    public virtual bool IsModalSupported(object context, Type viewModelType)
        => GetCreator(viewModelType, _ModalCreators) != null;

    public virtual Task OpenModalAsync(object context, object viewModel)
        => InvokeAsync(context, () => OpenModal(context, viewModel));

    protected virtual Task OpenModal(object context, object viewModel)
        => ShowModalAsync(context, CreateModalContent(viewModel));

    protected virtual FrameworkElement CreateModalContent(object viewModel)
        => GetCreator(
            viewModel?.GetType() ?? throw new ArgumentNullException(nameof(viewModel)),
            _ModalCreators)
        ?.Invoke(viewModel) ?? throw new ArgumentException(nameof(viewModel));

    protected virtual Task ShowModalAsync(object context, FrameworkElement frameworkElement)
    {
        ConfigureViewModel(frameworkElement.DataContext);

        if (frameworkElement is Window w)
        {
            w.Owner = GetWindow(context);

            w.ShowDialog();
            return Task.CompletedTask;
        }
        if (frameworkElement.Parent != null)
        {
            throw new InvalidOperationException();
        }
        var cw = new Window()
        {
            SizeToContent = SizeToContent.WidthAndHeight,
            ResizeMode = ResizeMode.CanResizeWithGrip,
            ShowInTaskbar = false,
            Owner = GetWindow(context),
            DataContext = frameworkElement.DataContext,
            Content = frameworkElement,
        };
        cw.ShowDialog();
        return Task.CompletedTask;
    }

    protected virtual void ConfigureViewModel(object viewModel)
    {
        if (viewModel is WindowViewModel wvm)
        {
            if (!wvm.HasInteraction)
            {
                wvm.Interaction = this;
            }
        }
    }

    public virtual Task CloseModalAsync(object context, object viewModel)
        => InvokeAsync(context, () => CloseModal(context, viewModel));

    protected virtual bool CloseModal(object context, object viewModel)
    {
        var w = (viewModel as IHasWindow)?.Window
            ?? Application.Current?.Windows.OfType<Window>().FirstOrDefault(e => e.DataContext == viewModel);

        if (w != null)
        {
            w.Close();
            return true;
        }
        return false;
    }

    #endregion モーダル

    #region ダウンロード

    protected virtual HttpClient GetHttpClient() => new HttpClient();

    protected virtual bool ShouldDisposeHttpClient => true;

    public virtual bool SupportsDownload => true;

    public Task DownloadAsync(
        object context,
        string method,
        string url,
        string content,
        string contentType,
        bool openFile)
        => DownloadAsync(context, method, url, content, contentType, openFile, null);

    public async Task DownloadAsync(
        object context,
        string method,
        string url,
        string content,
        string contentType,
        bool openFile,
        Action<string> opener)
    {
        var file = await DownloadCore(context, method, url, content, contentType, openFile);

        if (openFile)
        {
            (opener ?? Open)(file.FullName);
        }
    }

    protected virtual async Task<FileInfo> DownloadCore(
        object context,
        string method,
        string url,
        string content,
        string contentType,
        bool openFile)
    {
        var m = new HttpRequestMessage(new HttpMethod(method), url);
        if (!string.IsNullOrEmpty(content))
        {
            m.Content = new StringContent(content, Encoding.UTF8, contentType ?? "text/plain");
        }

        OnMessageCreated(m);

        var hc = GetHttpClient();
        try
        {
            var res = await hc.SendAsync(m, HttpCompletionOption.ResponseHeadersRead);

            res.EnsureSuccessStatusCode();
            var lm = res.Content.Headers.LastModified?.ToUniversalTime();

            var coreFile = await GetDownloadPathAsync(res, openFile);
            var i = 1;
            var file = new FileInfo(coreFile);
            while (file.Exists)
            {
                if (file.Length == res.Content.Headers.ContentLength
                    && file.LastWriteTimeUtc == lm)
                {
                    return file;
                }
                else
                {
                    file = new FileInfo(
                        Path.Combine(
                            file.Directory.FullName,
                            Path.GetFileNameWithoutExtension(coreFile)
                            + (++i).ToString("' ('0')'")
                            + Path.GetExtension(coreFile)));
                }
            }

            using (var cs = await res.Content.ReadAsStreamAsync())
            using (var fs = file.Open(FileMode.Create))
            {
                await cs.CopyToAsync(fs);
            }
            file.Refresh();
            if (lm != null)
            {
                file.LastWriteTimeUtc = lm.Value.ToUniversalTime().DateTime;
            }

            return file;
        }
        finally
        {
            if (ShouldDisposeHttpClient)
            {
                hc?.Dispose();
            }
        }
    }

    protected virtual void OnMessageCreated(HttpRequestMessage message)
    {
    }

    protected virtual Task<string> GetDownloadPathAsync(HttpResponseMessage response, bool openFile)
    {
        var fn = response.Content.Headers.ContentDisposition.FileNameStar
            ?? response.Content.Headers.ContentDisposition.FileName;

        if (fn == null)
        {
            var un = Path.GetFileName(response.Headers.Location.AbsolutePath);

            if (!string.IsNullOrEmpty(Path.GetExtension(un)))
            {
                fn = un;
            }
        }
        return GetDownloadPathAsync(response, fn, openFile);
    }

    protected virtual Task<string> GetDownloadPathAsync(HttpResponseMessage response, string coreFileName, bool openFile)
    {
        if (string.IsNullOrEmpty(coreFileName))
        {
            return Task.FromResult(
                    Path.GetTempFileName()
                    + GuessExtension(response.Content.Headers.ContentType?.MediaType));
        }
        else
        {
            return Task.FromResult(Path.Combine(Path.GetTempPath(), coreFileName));
        }
    }

    protected virtual void Open(string fileName)
        => Process.Start(fileName);

    protected virtual string GuessExtension(string contentType)
        => contentType switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/gif" => ".gif",
            "image/bmp" => ".bmp",
            "image/ico" => ".ico",
            "image/svg+xml" => ".svg",

            "text/plain" => ".txt",
            "text/csv" => ".csv",
            "text/html" => ".html",
            "text/css" => ".css",

            "application/json" => ".json",
            "application/pdf" => ".pdf",
            "application/zip" => ".zip",

            "application/vnd.ms-excel" => ".xls",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => ".xlsx",

            "application/vnd.ms-powerpoint" => ".ppt",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation" => ".pptx",

            "application/msword" => ".doc",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => ".docx",

            "application/x-ms-application" => ".application",
            "application/x-ms-manifest" => ".manifest",

            _ => null
        };

    #endregion ダウンロード
}
