namespace Shipwreck.ViewModelUtils.Components;

public abstract class FrameworkLayoutBase : BindableLayoutComponentBase<FrameworkLayoutViewModel>, IHasJSRuntime
{
    [Inject]
    public IJSRuntime JS { get; set; }
}
