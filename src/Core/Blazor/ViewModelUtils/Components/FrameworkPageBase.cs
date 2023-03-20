namespace Shipwreck.ViewModelUtils.Components;

public abstract class FrameworkPageBase : BindableComponentBase<FrameworkPageViewModel>, IHasJSRuntime
{
    [Inject]
    public IJSRuntime JS { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

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

    #endregion DataContext

    protected override async Task OnParametersSetAsync()
    {
        var t = base.OnParametersSetAsync();

        if (t != null)
        {
            await t.ConfigureAwait();
        }

        using (CreateInitializingScope())
        {
            InitializeDataContext();

            var dc = DataContext;
            if (dc != null)
            {
                await dc.InitializeAsync().ConfigureAwait();
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var t = base.OnAfterRenderAsync(firstRender);
        if (t != null)
        {
            await t.ConfigureAwait();
        }
        var dc = DataContext;
        if (dc != null)
        {
            await dc.OnAfterRenderAsync(firstRender).ConfigureAwait();
            dc.IsVisible = true;
        }
    }
}
