namespace Shipwreck.ViewModelUtils.Components;

public abstract class FrameworkPageBase : BindableComponentBase<FrameworkPageViewModel>
    , IHasJSRuntime
    , IHasNavigationManager
    , IHasFrameworkPageViewModel
    , IDisposable
{
    [Inject]
    public IServiceProvider ServiceProvider { get; set; }

    [Inject]
    public IJSRuntime JS { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    #region PersistentComponentState

    private PersistentComponentState _PersistentState;

    public PersistentComponentState PersistentState
        => _PersistentState ??= ServiceProvider?.GetService(typeof(PersistentComponentState)) as PersistentComponentState;

    #endregion PersistentComponentState

    private PersistingComponentStateSubscription _PersistingSubscription;

    protected virtual IDisposable CreateInitializingScope() => null;

    #region DataContext

    protected override void OnDataContextSet(FrameworkPageViewModel dataContext)
    {
        base.OnDataContextSet(dataContext);

        if (dataContext != null)
        {
            dataContext.RequestFocus += DataContext_RequestFocus;
        }
    }

    protected override void OnDataContextRemoved(FrameworkPageViewModel dataContext)
    {
        base.OnDataContextRemoved(dataContext);

        if (dataContext != null)
        {
            dataContext.RequestFocus -= DataContext_RequestFocus;
            dataContext.Dispose();
        }
    }

    private void DataContext_RequestFocus(object sender, PropertyChangedEventArgs e)
    {
        if (sender == DataContext)
        {
            OnDataContextRequestedFocus(e.PropertyName);
        }
    }

    protected virtual FrameworkPageViewModel GetOrCreateDataContext()
        => base.DataContext;

    protected virtual void InitializeDataContext()
    {
        DataContext = GetOrCreateDataContext();
    }

    protected virtual void SetDataContextParameter()
    {
        DataContext = GetOrCreateDataContext();
    }

    #endregion DataContext

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _PersistingSubscription = PersistentState?.RegisterOnPersisting(OnPersistingAsync) ?? default;
    }

    private Task OnPersistingAsync()
    {
        DataContext.OnPersisting();
        return Task.CompletedTask;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        using (CreateInitializingScope())
        {
            InitializeDataContext();

            DataContext.TryTakeFromJson();
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        using (CreateInitializingScope())
        {
            InitializeDataContext();

            var dc = DataContext;
            if (dc != null)
            {
                await dc.InitializeAsync();
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var t = base.OnAfterRenderAsync(firstRender);
        if (t != null)
        {
            await t;
        }
        var dc = DataContext;
        if (dc != null)
        {
            await dc.OnAfterRenderAsync(firstRender);
            dc.IsVisible = true;
        }
    }

    #region IHasFrameworkPageViewModel

    FrameworkPageViewModel IHasFrameworkPageViewModel.Page => DataContext;

    IInteractionService IHasInteractionService.Interaction => DataContext?.Interaction;

    #endregion


    #region IDisposable

    protected bool IsDisposed { get; set; }

    protected virtual void Dispose(bool disposing)
    {
        _PersistingSubscription.Dispose();
        IsDisposed = true;
    }

#pragma warning disable CA1063 // Implement IDisposable Correctly

    public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
            => Dispose(true);

    #endregion IDisposable
}
