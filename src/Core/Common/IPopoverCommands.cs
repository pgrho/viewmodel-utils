namespace Shipwreck.ViewModelUtils;

public interface IPopoverCommands
{
    CommandViewModelCollection PrimaryCommands { get; }
    CommandViewModelCollection StateCommands { get; }
    CommandViewModelCollection PrintCommands { get; }
}
