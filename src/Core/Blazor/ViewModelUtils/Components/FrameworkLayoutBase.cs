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
        internal set => SetProperty(ref _PageContext, value);
    }

    #endregion PageContext
    public abstract ModalPresenterBase ModalPresenter { get; }

    public abstract ModalPresenterBase PopoverPresenter { get; }
}
