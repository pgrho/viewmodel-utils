namespace Shipwreck.ViewModelUtils.Components;

public class EnumFormGroup<T> : SelectFormGroup<T>
    where T : struct, Enum
{
    #region OptionFilter

    private Func<T, bool> _OptionFilter;

    [Parameter]
    public Func<T, bool> OptionFilter
    {
        get => _OptionFilter;
        set => SetProperty(ref _OptionFilter, value);
    }

    #endregion OptionFilter

    protected override IEnumerable<KeyValuePair<string, string>> CreateOptions()
        => Enum.GetValues(typeof(T)).Cast<T>()
            .Where(e => _OptionFilter?.Invoke(e) != false)
            .Select(e => new KeyValuePair<string, string>(e.ToString("g"), EnumMemberDisplayNames<T>.Default.GetValue(e)));
}
