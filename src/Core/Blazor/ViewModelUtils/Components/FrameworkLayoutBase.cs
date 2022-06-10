namespace Shipwreck.ViewModelUtils.Components;

public abstract class FrameworkLayoutBase : BindableLayoutComponentBase<FrameworkLayoutViewModel>, IHasJSRuntime, IHasInteractionService, IHasModalPresenter, IHasPopoverPresenter
{
    [Inject]
    public IJSRuntime JS { get; set; }

    [Inject]
    public IInteractionService Interaction { get; set; }

    #region Page

    private FrameworkPageBase _Page;

    public FrameworkPageBase Page
    {
        get => _Page;
        protected internal set
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

    #region InitialPageContext

    private WeakReference<FrameworkPageViewModel> _InitialPageContext;

    protected FrameworkPageViewModel InitialPageContext
    {
        get => _InitialPageContext is var f && f != null && f.TryGetTarget(out var ipc) ? ipc : null;
        set
        {
            if (value == null)
            {
                _InitialPageContext = null;
            }
            else if (_InitialPageContext == null)
            {
                _InitialPageContext = new WeakReference<FrameworkPageViewModel>(value);
            }
            else
            {
                _InitialPageContext.SetTarget(value);
            }
        }
    }

    #endregion InitialPageContext

    public abstract ModalPresenterBase ModalPresenter { get; }

    public abstract ModalPresenterBase PopoverPresenter { get; }

    protected override Task OnInitializedAsync()
    {
        InitialPageContext = PageContext;
        return base.OnInitializedAsync();
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        var ipc = InitialPageContext;

        var t = base.OnAfterRenderAsync(firstRender);

        if (ipc != _PageContext && _PageContext != null)
        {
            Task.Delay(1).ContinueWith(t => StateHasChanged());
        }

        return t;
    }
}
