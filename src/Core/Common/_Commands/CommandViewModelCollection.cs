namespace Shipwreck.ViewModelUtils;

public class CommandViewModelCollection : BulkUpdateableCollection<CommandViewModelBase>
{
    private sealed class Observer : CollectionObserver<CommandViewModelBase, (int visible, int enabled, CommandViewModelBase? first)>
    {
        protected override bool OnItemPropertyChanged(string propertyName)
            => propertyName == nameof(CommandViewModelBase.IsVisible)
            || propertyName == nameof(CommandViewModelBase.IsEnabled);

        protected override (int visible, int enabled, CommandViewModelBase? first) Calculate()
        {
            int v = 0, e = 0;
            CommandViewModelBase? first = null;
            if (Source != null)
            {
                foreach (var c in Source)
                {
                    if (c.IsVisible)
                    {
                        first = first ?? c;
                        v++;
                        if (c.IsEnabled)
                        {
                            e++;
                        }
                    }
                }
            }
            return (v, e, first);
        }

        protected override void OnValueChanged()
        {
            base.OnValueChanged();
            (Source as CommandViewModelCollection)?.InvalidateCounts();
        }
    }

    public CommandViewModelCollection()
    {
        _Observer = new Observer
        {
            Source = this
        };
    }

    public CommandViewModelCollection(IEnumerable<CommandViewModelBase> items)
        : base(items)
    {
        _Observer = new Observer
        {
            Source = this
        };

        if (Count > 0)
        {
            InvalidateCounts();
        }
    }

    private readonly Observer _Observer;

    #region VisibleCount

    private int _VisibleCount;

    public int VisibleCount
    {
        get => _VisibleCount;
        private set
        {
            var iv = IsVisible;
            if (SetProperty(ref _VisibleCount, value))
            {
                if (iv != IsVisible)
                {
                    RaisePropertyChanged(nameof(IsVisible));
                }
            }
        }
    }

    public bool IsVisible => _VisibleCount > 0;

    #endregion VisibleCount

    #region EnabledCount

    private int _EnabledCount;

    public int EnabledCount
    {
        get => _EnabledCount;
        private set => SetProperty(ref _EnabledCount, value);
    }

    #endregion EnabledCount

    #region FirstCommand

    private CommandViewModelBase? _FirstCommand;

    public CommandViewModelBase? FirstCommand
    {
        get => _FirstCommand;
        private set => SetProperty(ref _FirstCommand, value);
    }

    #endregion FirstCommand

    public void Invalidate()
    {
        foreach (var c in this)
        {
            c.Invalidate();
        }
        InvalidateCounts();
    }

    private void InvalidateCounts()
    {
        var c = _Observer.Value;
        VisibleCount = c.visible;
        EnabledCount = c.enabled;
        FirstCommand = c.first;
    }
}
