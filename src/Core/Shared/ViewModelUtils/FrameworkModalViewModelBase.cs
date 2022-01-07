namespace Shipwreck.ViewModelUtils;

public abstract partial class FrameworkModalViewModelBase : ObservableModel, IFrameworkModalViewModel, IHasFrameworkPageViewModel
{
    protected FrameworkModalViewModelBase(FrameworkPageViewModel page)
    {
        Page = page;
    }

    public FrameworkPageViewModel Page { get; }
    public IPageLogger Logger => Page?.Logger;
    public IInteractionService Interaction => Page?.Interaction;
    public bool IsDisposed { get; protected set; }

    public Task OpenAsync()
    {
        if (Page.IsModalSupported(GetType()) == true)
        {
            return Page.OpenModalAsync(this);
        }
        return Task.FromException(new NotSupportedException());
    }

    public virtual Task CloseAsync() => Page.CloseModalAsync(this);

    #region CancelCommand

    private CommandViewModelBase _CancelCommand;

    public CommandViewModelBase CancelCommand
        => _CancelCommand ??= CreateCancelCommand();

    protected virtual CommandViewModelBase CreateCancelCommand()
        => CommandViewModel.CreateAsync(
            CloseAsync,
            title: SR.CancelTitle,
            style: BorderStyle.OutlineSecondary);

    #endregion CancelCommand

    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
        }
    }

    public void Dispose() => Dispose(disposing: true);
}
