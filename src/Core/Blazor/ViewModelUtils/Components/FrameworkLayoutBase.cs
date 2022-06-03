namespace Shipwreck.ViewModelUtils.Components;

public abstract class FrameworkLayoutBase : BindableLayoutComponentBase<FrameworkLayoutViewModel>, IHasJSRuntime, IHasInteractionService, IHasModalPresenter, IHasPopoverPresenter
{
    private WeakReference<FrameworkPageViewModel> _InitialPageContext;

    [Inject]
    public IJSRuntime JS { get; set; }

    [Inject]
    public IInteractionService Interaction { get; set; }

    #region Page

    private FrameworkPageBase _Page;

    public FrameworkPageBase Page
    {
        get => _Page;
        internal set
        {
            SetProperty(ref _Page, value);
            OnPageChanged();
        }
    }

    protected virtual void OnPageChanged()
    {
        PageContext = _Page?.DataContext;
        ModalPresenter?.CloseModal();
        PopoverPresenter?.CloseModal();
    }

    #endregion Page

    #region PageContext

    private FrameworkPageViewModel _PageContext;

    public FrameworkPageViewModel PageContext
    {
        get => _PageContext;
        protected internal set => SetProperty(ref _PageContext, value);
    }

    #endregion PageContext

    public abstract ModalPresenterBase ModalPresenter { get; }

    public abstract ModalPresenterBase PopoverPresenter { get; }

    protected override Task OnInitializedAsync()
    {
        _InitialPageContext = _PageContext != null ? new WeakReference<FrameworkPageViewModel>(_PageContext) : null;
        return base.OnInitializedAsync();
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        FrameworkPageViewModel ipc = null;
        _InitialPageContext?.TryGetTarget(out ipc);

        var t = base.OnAfterRenderAsync(firstRender);

        if (ipc != _PageContext)
        {
            Task.Delay(1).ContinueWith(t => StateHasChanged());
        }

        return t;
    }
}
