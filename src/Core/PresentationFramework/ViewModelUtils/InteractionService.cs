using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace Shipwreck.ViewModelUtils
{
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
            => MessageBox.Show(GetWindow(context), message, title);

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
                null,
                default,
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
                Filter = fileName,
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
                Filter = fileName,
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

        private readonly Dictionary<Type, Func<object, Window>> _WindowCreators
            = new Dictionary<Type, Func<object, Window>>();

        public void RegisterWindow(Type viewModelType, Func<object, Window> handler)
        {
            lock (((ICollection)_WindowCreators).SyncRoot)
            {
                _WindowCreators[viewModelType ?? throw new ArgumentNullException(nameof(viewModelType))]
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
            => GetCreator(viewModelType, _WindowCreators) != null;

        public virtual Task OpenModalAsync(object context, object viewModel)
            => InvokeAsync(context, () => OpenModal(context, viewModel));

        protected virtual bool? OpenModal(object context, object viewModel)
        {
            var w = GetCreator(
                viewModel?.GetType() ?? throw new ArgumentNullException(nameof(viewModel)),
                _WindowCreators)
                ?.Invoke(viewModel) ?? throw new ArgumentException(nameof(viewModel));

            w.Owner = GetWindow(context);

            return w.ShowDialog();
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
    }
}
