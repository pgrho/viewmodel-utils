namespace Shipwreck.ViewModelUtils
{
    public partial interface IHasFrameworkPageViewModel : IHasInteractionService, IHasPageLogger
    {
        FrameworkPageViewModel Page { get; }

#if NETCOREAPP3_0_OR_GREATER
        IPageLogger IHasPageLogger.Logger => Page?.Logger;

        IInteractionService IHasInteractionService.Interaction => Page?.Interaction;
#endif
    }
}
