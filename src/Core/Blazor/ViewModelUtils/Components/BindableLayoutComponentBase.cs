namespace Shipwreck.ViewModelUtils.Components;

public abstract partial class BindableLayoutComponentBase : LayoutComponentBase, ComponentUpdateScope.IComponent
{
    protected bool IsUpdatingSource { get; set; }

    #region ShouldRenderCore

    private bool _ShouldRenderCore = true;

    protected bool ShouldRenderCore
    {
        get => _ShouldRenderCore;
        set
        {
            if (value != _ShouldRenderCore)
            {
                _ShouldRenderCore = value;
                if (_ShouldRenderCore)
                {
                    base.StateHasChanged();
                }
            }
        }
    }

    protected override bool ShouldRender() => _ShouldRenderCore;

    #endregion ShouldRenderCore

    #region BindableComponentBase

    private List<WeakReference<ComponentUpdateScope>> _Scopes;
    private bool _IsChangeDefered;

    protected ComponentUpdateScope BeginUpdate()
    {
        var s = new ComponentUpdateScope(this);
        (_Scopes ?? (_Scopes = new List<WeakReference<ComponentUpdateScope>>(4))).Add(new WeakReference<ComponentUpdateScope>(s));
        return s;
    }

    void ComponentUpdateScope.IComponent.EndUpdate(ComponentUpdateScope scope)
    {
        if (!ComponentUpdateScope.HasScopes(_Scopes, scope) && _IsChangeDefered)
        {
            _IsChangeDefered = false;
            ShouldRenderCore = true;
        }
    }

    protected new void StateHasChanged()
    {
        if (ComponentUpdateScope.HasScopes(_Scopes))
        {
            _IsChangeDefered = true;
        }
        else
        {
            _IsChangeDefered = false;
            ShouldRenderCore = true;
        }
    }

    #endregion BindableComponentBase

    [CascadingParameter(Name = nameof(IBindableComponent))]
    public IBindableComponent Host { get; set; }

    public static string HostParameterName => nameof(IBindableComponent);

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        var t = base.OnAfterRenderAsync(firstRender);
        ShouldRenderCore = false;
        return t;
    }
}

public abstract partial class BindableLayoutComponentBase<T> : BindableLayoutComponentBase, IBindableComponent
    where T : class
{
    object IBindableComponent.DataContext => DataContext;
}
