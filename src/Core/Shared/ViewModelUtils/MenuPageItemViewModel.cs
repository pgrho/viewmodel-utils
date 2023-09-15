namespace Shipwreck.ViewModelUtils;

public sealed class MenuPageItemViewModel : ObservableModel
{
    public MenuPageItemViewModel(MenuPageGroupViewModel group, CommandViewModelBase command, bool isExtension = false)
    {
        Group = group;
        Command = command;
        BadgeCommands = Array.Empty<CommandViewModelBase>();
        SubCommands = new BulkUpdateableCollection<CommandViewModelBase>();
        IsExtension = isExtension;

        Command.PropertyChanged += Command_PropertyChanged;
    }

    public MenuPageGroupViewModel Group { get; }

    public CommandViewModelBase Command { get; }

    public bool IsExtension { get; }

    public IList<CommandViewModelBase> BadgeCommands { get; private set; }

    public BulkUpdateableCollection<CommandViewModelBase> SubCommands { get; }

    public MenuPageItemViewModel AddAction(CommandViewModelBase command)
        => Group.AddAction(command);

    public MenuPageItemViewModel AddBadge(CommandViewModelBase command)
    {
        if (BadgeCommands.Count == 0)
        {
            BadgeCommands = new List<CommandViewModelBase>(1) { command };
        }
        else
        {
            BadgeCommands.Add(command);
        }
        return this;
    }

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
    public void Invalidate()
    {
        Command?.Invalidate();

        foreach (var c in BadgeCommands)
        {
            c.Invalidate();
        }
        foreach (var c in SubCommands)
        {
            c.Invalidate();
        }
    }
}
