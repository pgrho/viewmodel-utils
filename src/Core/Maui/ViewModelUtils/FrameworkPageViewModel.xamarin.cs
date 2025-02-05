namespace Shipwreck.ViewModelUtils;

public partial class FrameworkPageViewModel
{
    public struct ProcessingDisabled : IDisposable
    {
        public void Dispose()
        {
        }
    }

    public ProcessingDisabled DisableProcessing()
        => default;

    static partial void PlatformGetInteractionService(ref IInteractionService service)
        => service = DependencyService.Get<IInteractionService>();

    partial void GetProcessType(ref ProcessType value)
        => value = ProcessType.XamarinForms;
}
