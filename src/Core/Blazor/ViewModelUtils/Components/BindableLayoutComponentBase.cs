﻿namespace Shipwreck.ViewModelUtils.Components;

public abstract partial class BindableLayoutComponentBase : LayoutComponentBase, ComponentUpdateScope.IComponent
{
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
            base.StateHasChanged();
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
            base.StateHasChanged();
        }
    }

    #endregion BindableComponentBase
}

public abstract partial class BindableLayoutComponentBase<T> : BindableLayoutComponentBase, IBindableComponent
    where T : class
{
    object IBindableComponent.DataContext => DataContext;
}
