namespace Shipwreck.ViewModelUtils;

public interface ICommandViewModelHandler
{
    void OnCommandExecuting(CommandViewModelBase command);

    void OnCommandExecuted(CommandViewModelBase command);
}
