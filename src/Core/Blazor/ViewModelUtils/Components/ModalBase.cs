namespace Shipwreck.ViewModelUtils.Components;

public abstract class ModalBase<T> : BindableComponentBase<T>, IModal, IHasJSRuntime
    where T : class
{
    public ElementReference ModalElement { get; set; }

    private bool _IsRendered;

    protected internal ModalPresenterBase Presenter { get; set; }

    ModalPresenterBase IModal.Presenter
    {
        get => Presenter;
        set => Presenter = value;
    }

    [Inject]
    public IJSRuntime JS { get; set; }

    #region IsOpen

    private bool _IsOpen;

    [Parameter]
    public bool IsOpen
    {
        get => _IsOpen;
        set
        {
            if (value != _IsOpen)
            {
                _IsOpen = value;
                OnIsOpenChanged();
            }
        }
    }

    protected virtual void OnIsOpenChanged()
    {
        if (_IsOpen || _IsRendered)
        {
            StateHasChanged();
        }
    }

    #endregion IsOpen

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var t = base.OnAfterRenderAsync(firstRender);
        if (t != null)
        {
            await t;
        }

        if (ModalElement.Id == null)
        {
            throw new Exception();
        }

        if (_IsOpen)
        {
            await ShowAsyncCore();
        }
        else if (_IsRendered)
        {
            await HideAsyncCore();
        }

        _IsRendered = true;
    }

    protected abstract ValueTask ShowAsyncCore();

    protected abstract ValueTask HideAsyncCore();

    [JSInvokable]
    public virtual void OnClosed()
    {
        _IsOpen = false;

        if (Presenter?.Modal == this)
        {
            Presenter.CloseModal();
        }
    }

    public void Dispose()
        => Dispose(false);

    protected virtual void Dispose(bool disposing) => IsOpen = false;
}
