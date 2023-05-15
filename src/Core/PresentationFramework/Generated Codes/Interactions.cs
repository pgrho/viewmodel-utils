namespace Shipwreck.ViewModelUtils
{
    partial class FrameworkPageViewModel
    {
        #region InvokeAsync

        public bool InvokeRequired => Interaction?.InvokeRequired ?? false;

        public Task InvokeAsync(Action action)
            => Interaction.InvokeAsync(this, action);

        public Task<T> InvokeAsync<T>(Func<T> func)
            => Interaction.InvokeAsync(this, func);

        #endregion InvokeAsync

        #region Logger
        public void LogVerbose(string message)
            => Logger?.Log(TraceEventType.Verbose, 0, message);
        public void LogVerbose(string format, params object[] args)
            => Logger?.Log(TraceEventType.Verbose, 0, format, args);
        public void LogVerbose(int id, string message)
            => Logger?.Log(TraceEventType.Verbose, id, message);
        public void LogVerbose(int id, string format, params object[] args)
            => Logger?.Log(TraceEventType.Verbose, id, format, args);
        public void LogInformation(string message)
            => Logger?.Log(TraceEventType.Information, 0, message);
        public void LogInformation(string format, params object[] args)
            => Logger?.Log(TraceEventType.Information, 0, format, args);
        public void LogInformation(int id, string message)
            => Logger?.Log(TraceEventType.Information, id, message);
        public void LogInformation(int id, string format, params object[] args)
            => Logger?.Log(TraceEventType.Information, id, format, args);
        public void LogWarning(string message)
            => Logger?.Log(TraceEventType.Warning, 0, message);
        public void LogWarning(string format, params object[] args)
            => Logger?.Log(TraceEventType.Warning, 0, format, args);
        public void LogWarning(int id, string message)
            => Logger?.Log(TraceEventType.Warning, id, message);
        public void LogWarning(int id, string format, params object[] args)
            => Logger?.Log(TraceEventType.Warning, id, format, args);
        public void LogError(string message)
            => Logger?.Log(TraceEventType.Error, 0, message);
        public void LogError(string format, params object[] args)
            => Logger?.Log(TraceEventType.Error, 0, format, args);
        public void LogError(int id, string message)
            => Logger?.Log(TraceEventType.Error, id, message);
        public void LogError(int id, string format, params object[] args)
            => Logger?.Log(TraceEventType.Error, id, format, args);
        public void LogCritical(string message)
            => Logger?.Log(TraceEventType.Critical, 0, message);
        public void LogCritical(string format, params object[] args)
            => Logger?.Log(TraceEventType.Critical, 0, format, args);
        public void LogCritical(int id, string message)
            => Logger?.Log(TraceEventType.Critical, id, message);
        public void LogCritical(int id, string format, params object[] args)
            => Logger?.Log(TraceEventType.Critical, id, format, args);
        #endregion Logger

        #region Toast

        public bool SupportsToasts => Interaction?.SupportsToasts ?? false;
        
        public Task ShowSuccessToastAsync(string message, string title = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                LogInformation("ShowSuccessToastAsync(\"{0}\")", message);
            }
            else
            {
                LogInformation("ShowSuccessToastAsync(\"{0}\", \"{1}\")", message, title);
            }
            return Interaction.ShowSuccessToastAsync(this, message, GetDefaultMessageTitle(title));
        }
        public Task ShowSuccessToastAsync(string format, object[] args, string title = null)
            => ShowSuccessToastAsync(string.Format(format, args), title: title);

        public Task ShowSuccessToastAsync(string format, params object[] args)
            => ShowSuccessToastAsync(string.Format(format, args), title: null);
        public Task ShowErrorToastAsync(string message, string title = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                LogInformation("ShowErrorToastAsync(\"{0}\")", message);
            }
            else
            {
                LogInformation("ShowErrorToastAsync(\"{0}\", \"{1}\")", message, title);
            }
            return Interaction.ShowErrorToastAsync(this, message, GetDefaultMessageTitle(title));
        }
        public Task ShowErrorToastAsync(string format, object[] args, string title = null)
            => ShowErrorToastAsync(string.Format(format, args), title: title);

        public Task ShowErrorToastAsync(string format, params object[] args)
            => ShowErrorToastAsync(string.Format(format, args), title: null);
        public Task ShowWarningToastAsync(string message, string title = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                LogInformation("ShowWarningToastAsync(\"{0}\")", message);
            }
            else
            {
                LogInformation("ShowWarningToastAsync(\"{0}\", \"{1}\")", message, title);
            }
            return Interaction.ShowWarningToastAsync(this, message, GetDefaultMessageTitle(title));
        }
        public Task ShowWarningToastAsync(string format, object[] args, string title = null)
            => ShowWarningToastAsync(string.Format(format, args), title: title);

        public Task ShowWarningToastAsync(string format, params object[] args)
            => ShowWarningToastAsync(string.Format(format, args), title: null);
        public Task ShowInformationToastAsync(string message, string title = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                LogInformation("ShowInformationToastAsync(\"{0}\")", message);
            }
            else
            {
                LogInformation("ShowInformationToastAsync(\"{0}\", \"{1}\")", message, title);
            }
            return Interaction.ShowInformationToastAsync(this, message, GetDefaultMessageTitle(title));
        }
        public Task ShowInformationToastAsync(string format, object[] args, string title = null)
            => ShowInformationToastAsync(string.Format(format, args), title: title);

        public Task ShowInformationToastAsync(string format, params object[] args)
            => ShowInformationToastAsync(string.Format(format, args), title: null);

        protected virtual string GetDefaultMessageTitle(string title)
            => !string.IsNullOrEmpty(title) ? title
            : Title;

        #endregion Toast

        #region メッセージ

        public bool SupportsMessageBoxes => Interaction?.SupportsMessageBoxes ?? false;

        public Task AlertAsync(
            string message,
            string title = null,
            string buttonText = null,
            BorderStyle? buttonStyle = null)
        {
            LogInformation("AlertAsync(\"{0}\")", message);
            var task = Interaction.AlertAsync(
                this,
                message,
                GetDefaultMessageTitle(title),
                buttonText,
                buttonStyle);

            task.ContinueWith(t =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    LogInformation("AlertAsync(\"{0}\") closed", message);
                }
                else if (t.Exception != null)
                {
                    LogError("AlertAsync(\"{0}\") Exception={1}", message, t.Exception);
                }
            }
#if IS_WEBVIEW
            , TaskScheduler.FromCurrentSynchronizationContext()
#endif
            );

            return task;
        }

        public Task<bool> ConfirmAsync(
            string message,
            string title = null,
            string trueText = null,
            BorderStyle? trueStyle = null,
            string falseText = null,
            BorderStyle? falseStyle = null)
        {
            LogInformation("ConfirmAsync(\"{0}\")", message);
            var task = Interaction.ConfirmAsync(
                this,
                message,
                GetDefaultMessageTitle(title),
                trueText,
                trueStyle,
                falseText,
                falseStyle);

            task.ContinueWith(t =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    LogInformation("ConfirmAsync(\"{0}\") result={1}", message, t.Result);
                }
                else if (t.Exception != null)
                {
                    LogError("ConfirmAsync(\"{0}\") Exception={1}", message, t.Exception);
                }
            }
#if IS_WEBVIEW
            , TaskScheduler.FromCurrentSynchronizationContext()
#endif
            );

            return task;
        }

        #endregion メッセージ

        #region ファイル

        public bool SupportsFileDialogs => Interaction?.SupportsFileDialogs ?? false;
        
        public Task<string> OpenFileAsync(
            string filter = null,
            int filterIndex = 0,
            string fileName = null,
            string initialDirectory = null)
            => OpenFilesAsync(
            filter: filter,
            filterIndex: filterIndex,
            fileName: fileName,
            initialDirectory: initialDirectory,
            multiSelect: false).ContinueWith(t => t.Result?.FirstOrDefault());

        public Task<string[]> OpenFilesAsync(
            string filter = null,
            int filterIndex = 0,
            string fileName = null,
            string initialDirectory = null,
            bool multiSelect = false)
            => Interaction.OpenFilesAsync(
            this,
            filter: filter,
            filterIndex: filterIndex,
            fileName: fileName,
            initialDirectory: initialDirectory,
            multiSelect: multiSelect);

        public Task<string> SaveFileAsync(
            string filter = null,
            int filterIndex = 0,
            string fileName = null,
            string initialDirectory = null)
            => Interaction.SaveFileAsync(
            this,
            filter: filter,
            filterIndex: filterIndex,
            fileName: fileName,
            initialDirectory: initialDirectory);

        public Task<string> OpenDirectoryAsync(
            string directoryName = null)
            => Interaction.OpenDirectoryAsync(
            this,
            directoryName: directoryName);

        public Task<string> SaveDirectoryAsync(
            string directoryName = null)
            => Interaction.OpenDirectoryAsync(
            this,
            directoryName: directoryName);

        #endregion ファイル

        #region モーダル

        public bool IsModalSupported(Type viewModelType)
            => Interaction.IsModalSupported(this, viewModelType);

        public Task OpenModalAsync(object viewModel)
            => Interaction.OpenModalAsync(this, viewModel);

        public Task CloseModalAsync(object viewModel)
            => Interaction.CloseModalAsync(this, viewModel);
            
        #endregion ファイル

        #region ダウンロード
        
        public bool SupportsDownload => Interaction?.SupportsDownload ?? false;
        
        public Task DownloadAsync(
            string url,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
            => DownloadAsync("GET", url, openFile: openFile, operationName: operationName, busySetter: busySetter);

        public Task DownloadAsync(
            string method,
            string url,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
            => DownloadAsync(method, url, null, null, openFile: openFile, operationName: operationName, busySetter: busySetter);

        public async Task DownloadAsync(
            string method,
            string url,
            string content,
            string contentType,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
        {
            operationName ??= "ダウンロード";
            try
            {
                LogInformation(
                    "{0}の開始: {1} {2}",
                    operationName,
                    method,
                    url);
                busySetter?.Invoke(true);

                await Interaction.DownloadAsync(
                    this,
                    method,
                    url,
                    content,
                    contentType,
                    openFile)
                    ;
            }
            catch (Exception ex)
            {
                LogError("{0}中にエラーが発生しました。{1}", operationName, ex);

                await ShowErrorToastAsync(
                    "{0}中にエラーが発生しました。{1}",
                    operationName,
                    ex.Message);
            }
            finally
            {
                busySetter?.Invoke(false);
                LogInformation(
                    "{0}の完了: {1} {2}",
                    operationName,
                    method,
                    url);
            }
        }
        
        public void BeginDownload(
            string url,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
            => BeginDownload("GET", url, openFile: openFile, operationName: operationName, busySetter: busySetter);

        public void BeginDownload(
            string method,
            string url,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
            => BeginDownload(method, url, null, null, openFile: openFile, operationName: operationName, busySetter: busySetter);

        public async void BeginDownload(
            string method,
            string url,
            string content,
            string contentType,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
        {
            operationName ??= "ダウンロード";
            try
            {
                await DownloadAsync(method, url, content, contentType, openFile: openFile, operationName: operationName, busySetter: busySetter);
            }
            catch (Exception ex)
            {
                LogError("{0}中にエラーが発生しました。{1}", operationName, ex);
            }
        }

        #endregion ダウンロード
    }
}
#if IS_WPF
namespace Shipwreck.ViewModelUtils
{
    partial class WindowViewModel
    {
        #region InvokeAsync

        public bool InvokeRequired => Interaction?.InvokeRequired ?? false;

        public Task InvokeAsync(Action action)
            => Interaction.InvokeAsync(this, action);

        public Task<T> InvokeAsync<T>(Func<T> func)
            => Interaction.InvokeAsync(this, func);

        #endregion InvokeAsync

        #region Logger
        public void LogVerbose(string message)
            => Logger?.Log(TraceEventType.Verbose, 0, message);
        public void LogVerbose(string format, params object[] args)
            => Logger?.Log(TraceEventType.Verbose, 0, format, args);
        public void LogVerbose(int id, string message)
            => Logger?.Log(TraceEventType.Verbose, id, message);
        public void LogVerbose(int id, string format, params object[] args)
            => Logger?.Log(TraceEventType.Verbose, id, format, args);
        public void LogInformation(string message)
            => Logger?.Log(TraceEventType.Information, 0, message);
        public void LogInformation(string format, params object[] args)
            => Logger?.Log(TraceEventType.Information, 0, format, args);
        public void LogInformation(int id, string message)
            => Logger?.Log(TraceEventType.Information, id, message);
        public void LogInformation(int id, string format, params object[] args)
            => Logger?.Log(TraceEventType.Information, id, format, args);
        public void LogWarning(string message)
            => Logger?.Log(TraceEventType.Warning, 0, message);
        public void LogWarning(string format, params object[] args)
            => Logger?.Log(TraceEventType.Warning, 0, format, args);
        public void LogWarning(int id, string message)
            => Logger?.Log(TraceEventType.Warning, id, message);
        public void LogWarning(int id, string format, params object[] args)
            => Logger?.Log(TraceEventType.Warning, id, format, args);
        public void LogError(string message)
            => Logger?.Log(TraceEventType.Error, 0, message);
        public void LogError(string format, params object[] args)
            => Logger?.Log(TraceEventType.Error, 0, format, args);
        public void LogError(int id, string message)
            => Logger?.Log(TraceEventType.Error, id, message);
        public void LogError(int id, string format, params object[] args)
            => Logger?.Log(TraceEventType.Error, id, format, args);
        public void LogCritical(string message)
            => Logger?.Log(TraceEventType.Critical, 0, message);
        public void LogCritical(string format, params object[] args)
            => Logger?.Log(TraceEventType.Critical, 0, format, args);
        public void LogCritical(int id, string message)
            => Logger?.Log(TraceEventType.Critical, id, message);
        public void LogCritical(int id, string format, params object[] args)
            => Logger?.Log(TraceEventType.Critical, id, format, args);
        #endregion Logger

        #region Toast

        public bool SupportsToasts => Interaction?.SupportsToasts ?? false;
        
        public Task ShowSuccessToastAsync(string message, string title = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                LogInformation("ShowSuccessToastAsync(\"{0}\")", message);
            }
            else
            {
                LogInformation("ShowSuccessToastAsync(\"{0}\", \"{1}\")", message, title);
            }
            return Interaction.ShowSuccessToastAsync(this, message, GetDefaultMessageTitle(title));
        }
        public Task ShowSuccessToastAsync(string format, object[] args, string title = null)
            => ShowSuccessToastAsync(string.Format(format, args), title: title);

        public Task ShowSuccessToastAsync(string format, params object[] args)
            => ShowSuccessToastAsync(string.Format(format, args), title: null);
        public Task ShowErrorToastAsync(string message, string title = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                LogInformation("ShowErrorToastAsync(\"{0}\")", message);
            }
            else
            {
                LogInformation("ShowErrorToastAsync(\"{0}\", \"{1}\")", message, title);
            }
            return Interaction.ShowErrorToastAsync(this, message, GetDefaultMessageTitle(title));
        }
        public Task ShowErrorToastAsync(string format, object[] args, string title = null)
            => ShowErrorToastAsync(string.Format(format, args), title: title);

        public Task ShowErrorToastAsync(string format, params object[] args)
            => ShowErrorToastAsync(string.Format(format, args), title: null);
        public Task ShowWarningToastAsync(string message, string title = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                LogInformation("ShowWarningToastAsync(\"{0}\")", message);
            }
            else
            {
                LogInformation("ShowWarningToastAsync(\"{0}\", \"{1}\")", message, title);
            }
            return Interaction.ShowWarningToastAsync(this, message, GetDefaultMessageTitle(title));
        }
        public Task ShowWarningToastAsync(string format, object[] args, string title = null)
            => ShowWarningToastAsync(string.Format(format, args), title: title);

        public Task ShowWarningToastAsync(string format, params object[] args)
            => ShowWarningToastAsync(string.Format(format, args), title: null);
        public Task ShowInformationToastAsync(string message, string title = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                LogInformation("ShowInformationToastAsync(\"{0}\")", message);
            }
            else
            {
                LogInformation("ShowInformationToastAsync(\"{0}\", \"{1}\")", message, title);
            }
            return Interaction.ShowInformationToastAsync(this, message, GetDefaultMessageTitle(title));
        }
        public Task ShowInformationToastAsync(string format, object[] args, string title = null)
            => ShowInformationToastAsync(string.Format(format, args), title: title);

        public Task ShowInformationToastAsync(string format, params object[] args)
            => ShowInformationToastAsync(string.Format(format, args), title: null);

        protected virtual string GetDefaultMessageTitle(string title)
            => !string.IsNullOrEmpty(title) ? title
            : ApplicationName;

        #endregion Toast

        #region メッセージ

        public bool SupportsMessageBoxes => Interaction?.SupportsMessageBoxes ?? false;

        public Task AlertAsync(
            string message,
            string title = null,
            string buttonText = null,
            BorderStyle? buttonStyle = null)
        {
            LogInformation("AlertAsync(\"{0}\")", message);
            var task = Interaction.AlertAsync(
                this,
                message,
                GetDefaultMessageTitle(title),
                buttonText,
                buttonStyle);

            task.ContinueWith(t =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    LogInformation("AlertAsync(\"{0}\") closed", message);
                }
                else if (t.Exception != null)
                {
                    LogError("AlertAsync(\"{0}\") Exception={1}", message, t.Exception);
                }
            }
#if IS_WEBVIEW
            , TaskScheduler.FromCurrentSynchronizationContext()
#endif
            );

            return task;
        }

        public Task<bool> ConfirmAsync(
            string message,
            string title = null,
            string trueText = null,
            BorderStyle? trueStyle = null,
            string falseText = null,
            BorderStyle? falseStyle = null)
        {
            LogInformation("ConfirmAsync(\"{0}\")", message);
            var task = Interaction.ConfirmAsync(
                this,
                message,
                GetDefaultMessageTitle(title),
                trueText,
                trueStyle,
                falseText,
                falseStyle);

            task.ContinueWith(t =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    LogInformation("ConfirmAsync(\"{0}\") result={1}", message, t.Result);
                }
                else if (t.Exception != null)
                {
                    LogError("ConfirmAsync(\"{0}\") Exception={1}", message, t.Exception);
                }
            }
#if IS_WEBVIEW
            , TaskScheduler.FromCurrentSynchronizationContext()
#endif
            );

            return task;
        }

        #endregion メッセージ

        #region ファイル

        public bool SupportsFileDialogs => Interaction?.SupportsFileDialogs ?? false;
        
        public Task<string> OpenFileAsync(
            string filter = null,
            int filterIndex = 0,
            string fileName = null,
            string initialDirectory = null)
            => OpenFilesAsync(
            filter: filter,
            filterIndex: filterIndex,
            fileName: fileName,
            initialDirectory: initialDirectory,
            multiSelect: false).ContinueWith(t => t.Result?.FirstOrDefault());

        public Task<string[]> OpenFilesAsync(
            string filter = null,
            int filterIndex = 0,
            string fileName = null,
            string initialDirectory = null,
            bool multiSelect = false)
            => Interaction.OpenFilesAsync(
            this,
            filter: filter,
            filterIndex: filterIndex,
            fileName: fileName,
            initialDirectory: initialDirectory,
            multiSelect: multiSelect);

        public Task<string> SaveFileAsync(
            string filter = null,
            int filterIndex = 0,
            string fileName = null,
            string initialDirectory = null)
            => Interaction.SaveFileAsync(
            this,
            filter: filter,
            filterIndex: filterIndex,
            fileName: fileName,
            initialDirectory: initialDirectory);

        public Task<string> OpenDirectoryAsync(
            string directoryName = null)
            => Interaction.OpenDirectoryAsync(
            this,
            directoryName: directoryName);

        public Task<string> SaveDirectoryAsync(
            string directoryName = null)
            => Interaction.OpenDirectoryAsync(
            this,
            directoryName: directoryName);

        #endregion ファイル

        #region モーダル

        public bool IsModalSupported(Type viewModelType)
            => Interaction.IsModalSupported(this, viewModelType);

        public Task OpenModalAsync(object viewModel)
            => Interaction.OpenModalAsync(this, viewModel);

        public Task CloseModalAsync(object viewModel)
            => Interaction.CloseModalAsync(this, viewModel);
            
        #endregion ファイル

        #region ダウンロード
        
        public bool SupportsDownload => Interaction?.SupportsDownload ?? false;
        
        public Task DownloadAsync(
            string url,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
            => DownloadAsync("GET", url, openFile: openFile, operationName: operationName, busySetter: busySetter);

        public Task DownloadAsync(
            string method,
            string url,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
            => DownloadAsync(method, url, null, null, openFile: openFile, operationName: operationName, busySetter: busySetter);

        public async Task DownloadAsync(
            string method,
            string url,
            string content,
            string contentType,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
        {
            operationName ??= "ダウンロード";
            try
            {
                LogInformation(
                    "{0}の開始: {1} {2}",
                    operationName,
                    method,
                    url);
                busySetter?.Invoke(true);

                await Interaction.DownloadAsync(
                    this,
                    method,
                    url,
                    content,
                    contentType,
                    openFile)
                    ;
            }
            catch (Exception ex)
            {
                LogError("{0}中にエラーが発生しました。{1}", operationName, ex);

                await ShowErrorToastAsync(
                    "{0}中にエラーが発生しました。{1}",
                    operationName,
                    ex.Message);
            }
            finally
            {
                busySetter?.Invoke(false);
                LogInformation(
                    "{0}の完了: {1} {2}",
                    operationName,
                    method,
                    url);
            }
        }
        
        public void BeginDownload(
            string url,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
            => BeginDownload("GET", url, openFile: openFile, operationName: operationName, busySetter: busySetter);

        public void BeginDownload(
            string method,
            string url,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
            => BeginDownload(method, url, null, null, openFile: openFile, operationName: operationName, busySetter: busySetter);

        public async void BeginDownload(
            string method,
            string url,
            string content,
            string contentType,
            bool openFile = true,
            string operationName = null,
            Action<bool> busySetter = null)
        {
            operationName ??= "ダウンロード";
            try
            {
                await DownloadAsync(method, url, content, contentType, openFile: openFile, operationName: operationName, busySetter: busySetter);
            }
            catch (Exception ex)
            {
                LogError("{0}中にエラーが発生しました。{1}", operationName, ex);
            }
        }

        #endregion ダウンロード
    }
}
#endif
