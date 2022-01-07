namespace Shipwreck.ViewModelUtils;

public interface IHasInteraction
{
    IPageLogger Logger { get; }
    IInteractionService Interaction { get; }

    #region InvokeAsync

    bool InvokeRequired { get; }
    Task InvokeAsync(Action action);
    Task<T> InvokeAsync<T>(Func<T> func);

    #endregion InvokeAsync

    #region Logger

    void LogVerbose(string message);
    void LogVerbose(string format, params object[] args);
    void LogVerbose(int id, string message);
    void LogVerbose(int id, string format, params object[] args);
    void LogInformation(string message);
    void LogInformation(string format, params object[] args);
    void LogInformation(int id, string message);
    void LogInformation(int id, string format, params object[] args);
    void LogWarning(string message);
    void LogWarning(string format, params object[] args);
    void LogWarning(int id, string message);
    void LogWarning(int id, string format, params object[] args);
    void LogError(string message);
    void LogError(string format, params object[] args);
    void LogError(int id, string message);
    void LogError(int id, string format, params object[] args);
    void LogCritical(string message);
    void LogCritical(string format, params object[] args);
    void LogCritical(int id, string message);
    void LogCritical(int id, string format, params object[] args);

    #endregion Logger

    #region Toast

    bool SupportsToasts { get; }

    Task ShowSuccessToastAsync(string message, string title = null);
    Task ShowSuccessToastAsync(string format, object[] args, string title = null);
    Task ShowSuccessToastAsync(string format, params object[] args);

    Task ShowErrorToastAsync(string message, string title = null);
    Task ShowErrorToastAsync(string format, object[] args, string title = null);
    Task ShowErrorToastAsync(string format, params object[] args);

    Task ShowWarningToastAsync(string message, string title = null);
    Task ShowWarningToastAsync(string format, object[] args, string title = null);
    Task ShowWarningToastAsync(string format, params object[] args);

    Task ShowInformationToastAsync(string message, string title = null);
    Task ShowInformationToastAsync(string format, object[] args, string title = null);
    Task ShowInformationToastAsync(string format, params object[] args);

    #endregion Toast

    #region メッセージ

    bool SupportsMessageBoxes { get; }

    Task AlertAsync(
        string message,
        string title = null,
        string buttonText = null,
        BorderStyle? buttonStyle = null);

    Task<bool> ConfirmAsync(
        string message,
        string title = null,
        string trueText = null,
        BorderStyle? trueStyle = null,
        string falseText = null,
        BorderStyle? falseStyle = null);

    #endregion メッセージ

    #region ファイル

    bool SupportsFileDialogs { get; }

    Task<string[]> OpenFilesAsync(
        string filter,
        int filterIndex = 0,
        string fileName = null,
        string initialDirectory = null,
        bool multiSelect = false);

    Task<string> SaveFileAsync(
        string filter,
        int filterIndex = 0,
        string fileName = null,
        string initialDirectory = null);

    Task<string> OpenDirectoryAsync(
        string directoryName = null);

    Task<string> SaveDirectoryAsync(string directoryName = null);

    #endregion ファイル

    #region モーダル

    bool IsModalSupported(Type viewModelType);

    Task OpenModalAsync(object viewModel);

    Task CloseModalAsync(object viewModel);

    #endregion モーダル
}
