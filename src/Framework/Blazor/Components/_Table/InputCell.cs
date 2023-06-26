using Microsoft.AspNetCore.Components.Rendering;

namespace Shipwreck.ViewModelUtils.Components;

public abstract class InputCell<T> : ComponentBase
{
    [Parameter]
    public bool IsEnabled { get; set; } = true;

    [Parameter]
    public bool IsChanged { get; set; }

    [Parameter]
    public string Placeholder { get; set; }

    [Parameter]
    public T Value { get; set; }

    [Parameter]
    public Action<T> ValueChanged { get; set; }

    [Parameter]
    public IDictionary<string,object> InputAttributes { get; set; }

    private string _InternalValue;

    internal string InternalValue
    {
        get => _InternalValue;
        set
        {
            if (value != _InternalValue)
            {
                _InternalValue = value;
                if (TryParse(_InternalValue, out var v))
                {
                    ValueChanged?.Invoke(v);
                }
            }
        }
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        InternalValue = ToString(Value);
    }

    protected virtual string ToString(T value)
        => value?.ToString();

    protected virtual bool TryParse(string s, out T result)
    {
        try
        {
            result = (T)((IConvertible)s).ToType(typeof(T), null);

            return true;
        }
        catch
        {
        }
        result = default;
        return false;
    }
}
