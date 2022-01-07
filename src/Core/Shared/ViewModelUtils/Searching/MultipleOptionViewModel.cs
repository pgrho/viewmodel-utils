namespace Shipwreck.ViewModelUtils.Searching;

public abstract partial class MultipleOptionViewModel : ObservableModel
{
    public string DisplayName { get; }

    protected MultipleOptionViewModel(string displayName, bool isSelected)
    {
        DisplayName = displayName;
        _IsSelected = isSelected;
    }

    #region IsSelected

    private bool _IsSelected;

    public bool IsSelected
    {
        get => _IsSelected;
        set
        {
            if (SetProperty(ref _IsSelected, value))
            {
                OnSelectionChanged();
            }
        }
    }

    protected abstract void OnSelectionChanged();

    #endregion IsSelected

    public abstract void Select();

    #region ToggleSelectionCommand

    private CommandViewModelBase _ToggleSelectionCommand;

    public CommandViewModelBase ToggleSelectionCommand
        => _ToggleSelectionCommand
        ??= CommandViewModel.Create(() => IsSelected = !IsSelected, DisplayName);

    #endregion ToggleSelectionCommand
}
public partial class MultipleOptionViewModel<T> : MultipleOptionViewModel
{
    public MultipleOptionViewModel(MultipleOptionConditionViewModel<T> condition, T value, string displayName, bool isSelected = false)
        : base(displayName, isSelected)
    {
        Condition = condition;
        Value = value;
    }

    public MultipleOptionConditionViewModel<T> Condition { get; }

    public T Value { get; }

    protected override void OnSelectionChanged() => Condition.DisplayText = null;

    public override void Select()
    {
        foreach (var op in Condition.Options)
        {
            op.IsSelected = op == this;
        }
    }
}
