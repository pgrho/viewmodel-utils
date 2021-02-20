using System;
using System.Threading.Tasks;

namespace Shipwreck.ViewModelUtils
{
    public interface IInterationService
    {
        #region Toast

        bool SupportsToasts { get; }

        Task ShowSuccessToastAsync(object context, string message);

        Task ShowErrorToastAsync(object context, string message);

        Task ShowWarningToastAsync(object context, string message);

        Task ShowInformationToastAsync(object context, string message);

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

        #region ダウンロード

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

        #endregion ダウンロード

        #region ダウンロード

        bool SupportsDownload { get; }

        Task DownloadAsync(
            object context,
            string method,
            string url,
            string content,
            string contentType,
            bool openFile);

        #endregion ダウンロード

        #region モーダル

        bool IsModalSupported(object context, Type viewModelType);

        Task OpenModalAsync(object context, object viewModel);

        Task CloseModalAsync(object context, object viewModel);

        #endregion モーダル
    }
}
