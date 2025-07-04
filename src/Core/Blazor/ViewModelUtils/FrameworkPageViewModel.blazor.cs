﻿namespace Shipwreck.ViewModelUtils;

public partial class FrameworkPageViewModel : IHasJSRuntime, IHasNavigationManager, IHasBindableComponent
{
    public struct ProcessingDisabled : IDisposable
    {
        public void Dispose()
        {
        }
    }

    protected FrameworkPageViewModel(FrameworkPageBase page)
    {
        Page = page;
    }

    public FrameworkPageBase Page { get; set; }

    public IJSRuntime JS => Page.JS;
    public NavigationManager NavigationManager => Page.NavigationManager;

    IBindableComponent IHasBindableComponent.Component => Page;

    public ProcessingDisabled DisableProcessing()
        => default;

    partial void PlatformCreateNavigaionService(ref INavigationService s)
        => s = new NavigationService(Page.NavigationManager);

    partial void GetProcessType(ref ProcessType value)
        => value = Page.JS is IJSInProcessRuntime ? ProcessType.BlazorWebAssembly : ProcessType.BlazorServer;

    protected internal virtual void OnPersisting() { }
    protected internal virtual bool TryTakeFromJson() => false;
}
