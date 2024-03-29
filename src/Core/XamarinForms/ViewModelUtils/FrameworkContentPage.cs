﻿namespace Shipwreck.ViewModelUtils;

public class FrameworkContentPage : ContentPage, IHasFrameworkPageViewModel
{
    private WeakReference<object> _ViewModel;

    private object ViewModel
    {
        get => _ViewModel != null && _ViewModel.TryGetTarget(out var r) ? r : null;
        set
        {
            var p = ViewModel;
            if (p != value)
            {
                if (value == null)
                {
                    _ViewModel = null;
                }
                else
                {
                    _ViewModel = new WeakReference<object>(value);
                }
                OnBindingContextChanged(value, p);
            }
        }
    }

    protected sealed override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        ViewModel = BindingContext;
    }

    protected virtual void OnBindingContextChanged(object bindingContext, object previousBindingContext)
    {
        if (previousBindingContext is IRequestFocus pr)
        {
            pr.RequestFocus -= BindingContext_RequestFocus;
        }
        if (bindingContext is IRequestFocus r)
        {
            r.RequestFocus += BindingContext_RequestFocus;
        }
    }

    private void BindingContext_RequestFocus(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        => OnFocusRequested(e.PropertyName);

    protected virtual void OnFocusRequested(string propertyName)
    {
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is FrameworkPageViewModel vm)
        {
            vm.IsVisible = true;
        }
        else if (BindingContext is FrameworkModalViewModelBase vm2)
        {
            vm2.IsVisible = true;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is FrameworkPageViewModel vm)
        {
            vm.IsVisible = false;
        }
        else if (BindingContext is FrameworkModalViewModelBase vm2)
        {
            vm2.IsVisible = false;
        }
    }

    #region IHasFrameworkPageViewModel

    FrameworkPageViewModel IHasFrameworkPageViewModel.Page => BindingContext as FrameworkPageViewModel;

    IPageLogger IHasPageLogger.Logger => (BindingContext as FrameworkPageViewModel)?.Logger;

    IInteractionService IHasInteractionService.Interaction => (BindingContext as FrameworkPageViewModel)?.Interaction;

    #endregion IHasFrameworkPageViewModel
}
