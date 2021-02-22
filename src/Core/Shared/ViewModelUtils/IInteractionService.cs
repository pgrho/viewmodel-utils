using System;
using System.Threading.Tasks;

namespace Shipwreck.ViewModelUtils
{
    public interface IInteractionService
    {
        #region InvokeAsync

        bool InvokeRequired { get; }

        Task InvokeAsync(object context, Action action);

        Task<T> InvokeAsync<T>(object context, Func<T> func);

        #endregion InvokeAsync

        #region Toast

        bool SupportsToasts { get; }

        Task ShowSuccessToastAsync(object context, string message, string title);

        Task ShowErrorToastAsync(object context, string message, string title);

        Task ShowWarningToastAsync(object context, string message, string title);

        Task ShowInformationToastAsync(object context, string message, string title);

        #endregion Toast

        #region メッセージ

        bool SupportsMessageBoxes { get; }

        Task AlertAsync(
            object context,
            string message,
            string title,
            string buttonText,
            BorderStyle? buttonStyle);

        Task<bool> ConfirmAsync(
            object context,
            string message,
            string title,
            string trueText,
            BorderStyle? trueStyle,
            string falseText,
            BorderStyle? falseStyle);

        #endregion メッセージ

        #region ファイル

        bool SupportsFileDialogs { get; }

        Task<string[]> OpenFilesAsync(
            object context,
            string filter,
            int filterIndex = 0,
            string fileName = null,
            string initialDirectory = null,
            bool multiSelect = false);

        Task<string> SaveFileAsync(
            object context,
            string filter,
            int filterIndex = 0,
            string fileName = null,
            string initialDirectory = null);

        Task<string> OpenDirectoryAsync(
            object context,
            string directoryName = null);

        Task<string> SaveDirectoryAsync(
            object context,
            string directoryName = null);

        #endregion ファイル

        #region モーダル

        bool IsModalSupported(object context, Type viewModelType);

        Task OpenModalAsync(object context, object viewModel);

        Task CloseModalAsync(object context, object viewModel);

        #endregion モーダル
    }
}
