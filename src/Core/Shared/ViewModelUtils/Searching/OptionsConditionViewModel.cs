namespace Shipwreck.ViewModelUtils.Searching;

public abstract class OptionsConditionViewModel<T> : ConditionViewModel, IOptionsConditionViewModel
{
    protected OptionsConditionViewModel(SearchPropertyViewModel property)
        : base(property)
    {
    }

    Type IOptionsConditionViewModel.ValueType => typeof(T);

    #region Value

    private T _Value;

    public T Value
    {
        get => _Value;
        set
        {
            if (SetProperty(ref _Value, value))
            {
                RaisePropertyChanged(nameof(StringValue));
                RaisePropertyChanged(nameof(SelectedOption));
            }
        }
    }

    public OptionViewModel<T> SelectedOption
    {
        get => Options.FirstOrDefault(e => Equals(e.Value, Value)) ?? Options.FirstOrDefault();
        set
        {
            var v = value ?? Options.FirstOrDefault();
            if (v != null)
            {
                Value = v.Value;
            }
        }
    }

    #endregion Value

    public string StringValue
    {
        get => GetString(_Value);
        set => Value = Parse(value);
    }

    protected abstract IEnumerable<OptionViewModel<T>> CreateOptions();

    protected OptionViewModel<T> CreateOption(T value, string displayName)
        => new OptionViewModel<T>(value, GetString(value), displayName);

    private ReadOnlyCollection<OptionViewModel<T>> _Options;

    public ReadOnlyCollection<OptionViewModel<T>> Options
        => _Options ??= Array.AsReadOnly(CreateOptions().ToArray());

    protected abstract string GetString(T value);

    protected abstract T Parse(string value);

    public override void SetValue(string @operator, string value)
        => Value = Parse(value);

    public override bool TryCreateDefaultValueExpression(out string @operator, out string defaultValue)
    {
        @operator = null;
        defaultValue = StringValue;
        if (string.IsNullOrEmpty(defaultValue))
        {
            defaultValue = null;
            return false;
        }

        return true;
    }
}
