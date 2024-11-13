namespace Shipwreck.ViewModelUtils;

public class MenuItemViewModel : ObservableModel, ICommandViewModel
{
    public MenuItemViewModel()
    {
    }

    public MenuItemViewModel(CommandViewModelBase command)
    {
        Command = command;
    }

    #region Title

    private string? _Title;

    public string? Title
    {
        get => _Title ?? CommandTitle;
        set => SetProperty(ref _Title, value);
    }

    #endregion Title

    #region Command

    private CommandViewModelBase? _Command;

    public CommandViewModelBase? Command
    {
        get => _Command;
        set
        {
            var old = _Command;
            if (SetProperty(ref _Command, value))
            {
                old?.RemovePropertyChanged(Command_PropertyChanged);
                _Command?.AddPropertyChanged(Command_PropertyChanged);

                RaisePropertyChanged(nameof(CommandTitle));
                RaisePropertyChanged(nameof(Mnemonic));
                RaisePropertyChanged(nameof(Href));
                RaisePropertyChanged(nameof(Description));
                RaisePropertyChanged(nameof(Icon));
                RaisePropertyChanged(nameof(Style));
                RaisePropertyChanged(nameof(IsVisible));
                RaisePropertyChanged(nameof(IsEnabled));
                RaisePropertyChanged(nameof(BadgeCount));
            }
        }
    }

    public string? CommandTitle => _Command?.Title;

    public string? Mnemonic => _Command?.Mnemonic;

    public string? Href => _Command?.Href;
    public string? Description => _Command?.Description;
    public string? Icon => _Command?.Icon;
    public BorderStyle Style => _Command?.Style ?? BorderStyle.None;
    public bool IsVisible => _Command?.IsVisible ?? true;
    public bool IsEnabled => _Command?.IsEnabled ?? true;
    public int BadgeCount => _Command?.BadgeCount ?? 0;

    private void Command_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Command.Title):
                RaisePropertyChanged(nameof(CommandTitle));
                break;

            case nameof(Command.Mnemonic):
                RaisePropertyChanged(nameof(Mnemonic));
                break;

            case nameof(Command.Href):
                RaisePropertyChanged(nameof(Href));
                break;

            case nameof(Command.Description):
                RaisePropertyChanged(nameof(Description));
                break;

            case nameof(Command.Icon):
                RaisePropertyChanged(nameof(Icon));
                break;

            case nameof(Command.Style):
                RaisePropertyChanged(nameof(Style));
                break;

            case nameof(Command.IsVisible):
                RaisePropertyChanged(nameof(IsVisible));
                break;

            case nameof(Command.IsEnabled):
                RaisePropertyChanged(nameof(IsEnabled));
                break;

            case nameof(Command.BadgeCount):
                RaisePropertyChanged(nameof(BadgeCount));
                break;
        }
    }

    #endregion Command

    #region ContainsVisible

    private bool _ContainsVisible = true;

    public bool ContainsVisible
    {
        get => _ContainsVisible;
        private set => SetProperty(ref _ContainsVisible, value);
    }

    #endregion ContainsVisible

    private BulkUpdateableCollection<MenuItemViewModel>? _Children;

    public BulkUpdateableCollection<MenuItemViewModel> Children
    {
        get => _Children ??= new();
        set
        {
            if (value != _Children)
            {
                var vs = value?.ToList();
                if (vs?.Count > 0)
                {
                    if (!Children.SequenceEqual(vs))
                    {
                        Children.Set(vs);
                    }
                }
                else
                {
                    _Children?.Clear();
                }
            }
        }
    }
    public void Invalidate()
    {
        if (_Children != null)
        {
            foreach (var c in _Children)
            {
                c?.Invalidate();
            }
        }
        _Command?.Invalidate();

        ContainsVisible = Command?.IsVisible == true || _Children?.Any(e => e?.ContainsVisible == true) == true;
    }

    void ICommandViewModel.Execute() => Command?.Execute();
}
