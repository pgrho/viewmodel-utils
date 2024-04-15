using Shipwreck.ReflectionUtils;

namespace Shipwreck.ViewModelUtils;

public partial class EnumMemberFilter<T, TValue> : EnumMemberFilterBase<T, TValue>
    where TValue : struct, Enum
{
    private TValue?[] _AllValues;

    private readonly Action<EnumMemberFilter<T, TValue>> _OnChanged;

    private readonly static string DEFAULT_DESCRIPTION = null;

    public EnumMemberFilter(Func<T, TValue?> selector, Action<EnumMemberFilter<T, TValue>> onChanged, string name = null, string description = null)
        : base(selector, name: name, description: description ?? DEFAULT_DESCRIPTION)
    {
        _OnChanged = onChanged;
    }

    protected override IEnumerable<TValue?> EnumerateValues()
        => _AllValues ??= Enum.GetValues(typeof(TValue)).Cast<TValue?>().Prepend(null).ToArray();

    protected override string GetValue(TValue? value)
    {
        if (value == null)
        {
            return "null";
        }
        return value.Value.ToString("D");
    }

    protected override string GetDisplayName(TValue? value)
    {
        if (value == null)
        {
            return string.Empty;
        }
        return EnumMemberDisplayNames<TValue>.Default.GetValue(value.Value);
    }

    protected override bool TryParse(string text, out TValue? value)
    {
        if (string.IsNullOrEmpty(text)
            || text.Equals("null", StringComparison.OrdinalIgnoreCase))
        {
            value = null;
            return true;
        }

        if (Enum.TryParse(text, out TValue v) 
            || EnumMemberDisplayNames<TValue>.Default.TryParseValue(text, out v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }
    protected override void OnChanged()
        => _OnChanged(this);
}
