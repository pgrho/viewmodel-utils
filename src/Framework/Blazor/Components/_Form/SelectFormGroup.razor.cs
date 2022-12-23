namespace Shipwreck.ViewModelUtils.Components;
using KeyboardEventArgs = Microsoft.AspNetCore.Components.Web.KeyboardEventArgs;

public partial class SelectFormGroup<T> : FormGroupBase
{
    #region SelectId

    private string _SelectId;

    [Parameter]
    public string SelectId
    {
        get => _SelectId ??= (FormGroupId + "--select");
        set => SetProperty(ref _SelectId, value);
    }

    #endregion SelectId

    #region Value

    private T _Value;

    [Parameter]
    public T Value
    {
        get => _Value;
        set
        {
            if (!Equals(value, _Value))
            {
                _Value = value;
                using (Host?.PushPropertyChangedExpectation(BindingPropertyName))
                {
                    ValueChanged?.Invoke(_Value);
                }
                if (!IsUpdatingSource)
                {
                    ShouldRenderCore = true;
                }
            }
        }
    }

    protected T InternalValue
    {
        get => Value;
        set
        {
            IsUpdatingSource = true;
            Value = value;
            IsUpdatingSource = false;
        }
    }

    [Parameter]
    public Action<T> ValueChanged { get; set; }

    [Parameter]
    public string BindingPropertyName { get; set; }

    #endregion Value

    #region Options

    private ReadOnlyCollection<KeyValuePair<string, string>> _Options;

    [Parameter]
    public IList<KeyValuePair<string, string>> Options
    {
        get => _Options ??= new(CreateOptions().ToArray());
        set
        {
            var vs = value ?? Array.Empty<KeyValuePair<string, string>>();
            if (_Options?.SequenceEqual(vs) != true)
            {
                _Options = new(vs.ToArray());
                ShouldRenderCore = true;
            }
        }
    }

    protected virtual void InvalidateOptions()
    {
        if (_Options != null)
        {
            _Options = null;
            ShouldRenderCore = true;
        }
    }

    protected virtual IEnumerable<KeyValuePair<string, string>> CreateOptions()
        => Enumerable.Empty<KeyValuePair<string, string>>();

    #endregion Options

    protected virtual void OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            EnterCommand?.Execute(EnterCommandParameter);
        }
    }
}
