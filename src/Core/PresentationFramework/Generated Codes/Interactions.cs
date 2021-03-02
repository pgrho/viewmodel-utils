using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
            return Interaction.ShowSuccessToastAsync(this, message, title ?? Title);
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
            return Interaction.ShowErrorToastAsync(this, message, title ?? Title);
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
            return Interaction.ShowWarningToastAsync(this, message, title ?? Title);
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
            return Interaction.ShowInformationToastAsync(this, message, title ?? Title);
        }
        public Task ShowInformationToastAsync(string format, object[] args, string title = null)
            => ShowInformationToastAsync(string.Format(format, args), title: title);

        public Task ShowInformationToastAsync(string format, params object[] args)
            => ShowInformationToastAsync(string.Format(format, args), title: null);

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
                title,
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
            });

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
                title,
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
            });

            return task;
        }

        #endregion メッセージ

        #region ファイル

        public bool SupportsFileDialogs => Interaction?.SupportsFileDialogs ?? false;

        public Task<string[]> OpenFilesAsync(
            string filter,
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
            string filter,
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
            => Interaction.OpenModalAsync(this, viewModel);

        #endregion モーダル
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
            return Interaction.ShowSuccessToastAsync(this, message, title ?? ApplicationName);
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
            return Interaction.ShowErrorToastAsync(this, message, title ?? ApplicationName);
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
            return Interaction.ShowWarningToastAsync(this, message, title ?? ApplicationName);
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
            return Interaction.ShowInformationToastAsync(this, message, title ?? ApplicationName);
        }
        public Task ShowInformationToastAsync(string format, object[] args, string title = null)
            => ShowInformationToastAsync(string.Format(format, args), title: title);

        public Task ShowInformationToastAsync(string format, params object[] args)
            => ShowInformationToastAsync(string.Format(format, args), title: null);

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
                title,
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
            });

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
                title,
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
            });

            return task;
        }

        #endregion メッセージ

        #region ファイル

        public bool SupportsFileDialogs => Interaction?.SupportsFileDialogs ?? false;

        public Task<string[]> OpenFilesAsync(
            string filter,
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
            string filter,
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
            => Interaction.OpenModalAsync(this, viewModel);

        #endregion モーダル
    }
}
#endif
