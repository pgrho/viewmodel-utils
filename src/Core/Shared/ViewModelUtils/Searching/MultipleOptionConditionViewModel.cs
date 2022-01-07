namespace Shipwreck.ViewModelUtils.Searching;

public abstract partial class MultipleOptionConditionViewModel<T> : ConditionViewModel, IMultipleOptionConditionViewModel
{
    public MultipleOptionConditionViewModel(SearchPropertyViewModel property)
        : base(property)
    {
    }

    Type IMultipleOptionConditionViewModel.ValueType => typeof(T);

    #region Options

    private BulkUpdateableCollection<MultipleOptionViewModel<T>> _Options;

    public BulkUpdateableCollection<MultipleOptionViewModel<T>> Options
    {
        get
        {
            if (_Options == null)
            {
                _Options = new BulkUpdateableCollection<MultipleOptionViewModel<T>>();
                InitializeOptions();
            }
            return _Options;
        }
    }

    protected abstract void InitializeOptions();

    #endregion Options

    #region IsSearching

    private bool _IsSearching;

    public bool IsSearching
    {
        get => _IsSearching;
        protected set => SetProperty(ref _IsSearching, value);
    }

    #endregion IsSearching

    #region DisplayText

    private string _DisplayText;

    public string DisplayText
    {
        get => _DisplayText ??= GetDisplayText();
        protected internal set => SetProperty(ref _DisplayText, value);
    }

    protected virtual string GetDisplayText()
    {
        var sc = Options.Where(e => e.IsSelected).ToList();
        if (sc.Count == 0)
        {
            return SR.NoneSelected;
        }

        if (sc.Count == Options.Count)
        {
            return SR.AllSelected;
        }

        if (sc.Count <= 3)
        {
            return string.Join(SR.ItemSeparator, sc.Select(e => e.DisplayName));
        }

        if (sc.Count + 2 >= Options.Count)
        {
            return string.Format(
                SR.ExceptForArg0,
                string.Join(SR.ItemSeparator, Options.Where(e => !e.IsSelected).Select(e => e.DisplayName)));
        }

        return string.Format(SR.ItemsOfCountArg0, sc.Count);
    }

    #endregion DisplayText

    public void SelectAll()
    {
        foreach (var op in Options)
        {
            op.IsSelected = true;
        }
    }

    public void UnselectAll()
    {
        foreach (var op in Options)
        {
            op.IsSelected = false;
        }
    }

    #region SelectionCommands

    private ReadOnlyCollection<CommandViewModelBase> _SelectionCommands;

    public ReadOnlyCollection<CommandViewModelBase> SelectionCommands
        => _SelectionCommands ??= Array.AsReadOnly(new[] { SelectAllCommand, UnselectAllCommand });

    #region SelectAllCommand

    private CommandViewModelBase _SelectAllCommand;

    public CommandViewModelBase SelectAllCommand
        => _SelectAllCommand ??= CommandViewModel.Create(SelectAll, title: SR.SelectAll);

    #endregion SelectAllCommand

    #region UnselectAllCommand

    private CommandViewModelBase _UnselectAllCommand;

    public CommandViewModelBase UnselectAllCommand
        => _UnselectAllCommand ??= CommandViewModel.Create(UnselectAll, title: SR.UnselectAll);

    #endregion UnselectAllCommand

    #endregion SelectionCommands
}
