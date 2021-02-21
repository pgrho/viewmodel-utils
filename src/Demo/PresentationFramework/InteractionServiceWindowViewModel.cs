namespace Shipwreck.ViewModelUtils.Demo.PresentationFramework
{
    public sealed class InteractionServiceWindowViewModel : WindowViewModel
    {
        public InteractionServiceWindowViewModel(IInteractionService interaction)
        {
            Interaction = interaction;
        }
    }
}
