namespace Shipwreck.ViewModelUtils.Searching;

public abstract partial class EntityConditionViewModel : ConditionViewModel
{
    protected EntityConditionViewModel(SearchPropertyViewModel property)
        : base(property)
    {
    }

    #region Selector

    private IEntitySelector _Selector;

    public IEntitySelector Selector
    {
        get
        {
            if (_Selector == null)
            {
                _Selector = CreateSelector();
                _Selector.PropertyChanged += _Selector_PropertyChanged;
            }
            return _Selector;
        }
    }

    public object SelectedId
    {
        get => Selector.SelectedId;
        set => Selector.SelectedId = value;
    }

    public object SelectedItem
    {
        get => Selector.SelectedItem;
        set => Selector.SelectedItem = value;
    }

    public bool IsSearching
        => Selector.IsSearching;

    private void _Selector_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Selector.SelectedId):
                RaisePropertyChanged(nameof(SelectedId));
                break;

            case nameof(Selector.SelectedItem):
                RaisePropertyChanged(nameof(SelectedItem));
                Host.OnConditionChanged(this);
                break;

            case nameof(Selector.IsSearching):
                RaisePropertyChanged(nameof(IsSearching));
                break;
        }
    }

    protected abstract IEntitySelector CreateSelector();

    #endregion Selector

    #region IsNull

    private bool _IsNull;

    public bool IsNull
    {
        get => _IsNull;
        set => SetProperty(ref _IsNull, value);
    }

    private const string NULL_PATTERN = @"^\{NULL\}$";
#if NET7_0_OR_GREATER

    [GeneratedRegex(NULL_PATTERN, RegexOptions.IgnoreCase)]
    private static partial Regex NullRegex();

#else
    private static readonly Regex _NullRegex = new(NULL_PATTERN, RegexOptions.IgnoreCase);
    private static Regex NullRegex() => _NullRegex;
#endif

    #endregion IsNull

    public override void SetValue(string @operator, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Selector.SelectedId = null;
            return;
        }

        if (string.IsNullOrEmpty(@operator))
        {
            if (value.StartsWith("="))
            {
                @operator = "=";
                value = value.Substring(1);
            }
        }

        IsNull = value != null
            && (string.IsNullOrEmpty(@operator) || @operator == "=")
            && NullRegex().IsMatch(value);
        if (!IsNull)
        {
            if (Selector.TryParseId(value, out var id))
            {
                Selector.SelectedId = id;
            }
            else
            {
                Selector.SelectedId = null;
            }
        }
    }

    public override bool HasValue
        => IsNull || Selector.IsValid(Selector.SelectedId);

    public override void AppendValueTo(StringBuilder builder)
    {
        if (IsNull)
        {
            builder.Append("{NULL}");
        }
        else
        {
            builder.Append(Selector.SelectedId);
        }
    }

    public override bool TryCreateDefaultValueExpression(out string @operator, out string defaultValue)
    {
        @operator = null;

        if (IsNull)
        {
            defaultValue = "{NULL}";
            return true;
        }

        defaultValue = Selector.SelectedId?.ToString();

        return defaultValue != null;
    }
}
