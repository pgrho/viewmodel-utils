using System;
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

        #region Toast

        public bool SupportsToasts => Interaction?.SupportsToasts ?? false;

        public Task ShowSuccessToastAsync(object context, string message)
            => Interaction.ShowSuccessToastAsync(this, message);

        public Task ShowErrorToastAsync(object context, string message)
            => Interaction.ShowErrorToastAsync(this, message);

        public Task ShowWarningToastAsync(object context, string message)
            => Interaction.ShowWarningToastAsync(this, message);

        public Task ShowInformationToastAsync(object context, string message)
            => Interaction.ShowInformationToastAsync(this, message);

        #endregion Toast

        #region メッセージ

        public bool SupportsMessageBoxes => Interaction?.SupportsMessageBoxes ?? false;

        public Task AlertAsync(
            string message,
            string title,
            string buttonText,
            BorderStyle? buttonStyle)
            => Interaction.AlertAsync(
                this,
                message,
                title,
                buttonText,
                buttonStyle);

        public Task<bool> ConfirmAsync(
            string message,
            string title,
            string trueText,
            BorderStyle? trueStyle,
            string falseText,
            BorderStyle? falseStyle)
            => Interaction.ConfirmAsync(
            this,
            message,
            title,
            trueText,
            trueStyle,
            falseText,
            falseStyle);

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

        #region Toast

        public bool SupportsToasts => Interaction?.SupportsToasts ?? false;

        public Task ShowSuccessToastAsync(object context, string message)
            => Interaction.ShowSuccessToastAsync(this, message);

        public Task ShowErrorToastAsync(object context, string message)
            => Interaction.ShowErrorToastAsync(this, message);

        public Task ShowWarningToastAsync(object context, string message)
            => Interaction.ShowWarningToastAsync(this, message);

        public Task ShowInformationToastAsync(object context, string message)
            => Interaction.ShowInformationToastAsync(this, message);

        #endregion Toast

        #region メッセージ

        public bool SupportsMessageBoxes => Interaction?.SupportsMessageBoxes ?? false;

        public Task AlertAsync(
            string message,
            string title,
            string buttonText,
            BorderStyle? buttonStyle)
            => Interaction.AlertAsync(
                this,
                message,
                title,
                buttonText,
                buttonStyle);

        public Task<bool> ConfirmAsync(
            string message,
            string title,
            string trueText,
            BorderStyle? trueStyle,
            string falseText,
            BorderStyle? falseStyle)
            => Interaction.ConfirmAsync(
            this,
            message,
            title,
            trueText,
            trueStyle,
            falseText,
            falseStyle);

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
