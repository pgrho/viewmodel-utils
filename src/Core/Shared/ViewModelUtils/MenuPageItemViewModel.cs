namespace Shipwreck.ViewModelUtils;

public sealed class MenuPageItemViewModel : ObservableModel
{
    public MenuPageItemViewModel(MenuPageGroupViewModel group, CommandViewModelBase command, bool isExtension = false)
    {
        Group = group;
        Command = command;
        SubCommands = new BulkUpdateableCollection<CommandViewModelBase>();
        IsExtension = isExtension;

        Command.PropertyChanged += Command_PropertyChanged;
    }

    public MenuPageGroupViewModel Group { get; }

    public CommandViewModelBase Command { get; }

    public bool IsExtension { get; }

    public BulkUpdateableCollection<CommandViewModelBase> SubCommands { get; }

    public MenuPageItemViewModel AddAction(CommandViewModelBase command)
        => Group.AddAction(command);

    public MenuPageItemViewModel AddSubAction(CommandViewModelBase command)
    {
        SubCommands.Add(command);

        return this;
    }

    private void Command_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Command.IsVisible))
        {
            Group.Invalidate();
        }
    }
}
