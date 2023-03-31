namespace Shipwreck.ViewModelUtils.Components;

public partial class InputFormGroup<T> : FormGroupBase
{
    #region InputId

    private string _InputId;

    [Parameter]
    public string InputId
    {
        get => _InputId ??= (FormGroupId + "--input");
        set => SetProperty(ref _InputId, value);
    }

    #endregion InputId

    #region DataListId

    private string _DataListId;

    [Parameter]
    public string DataListId
    {
        get => _DataListId ??= (FormGroupId + "--datalist");
        set => SetProperty(ref _DataListId, value);
    }

    #endregion DataListId

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
                    InvokeValueChanged();
                }
                if (!IsUpdatingSource)
                {
                    ShouldRenderCore = true;
                }
            }
        }
    }

    protected virtual void InvokeValueChanged()
        => ValueChanged?.Invoke(_Value);

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

    [Parameter]
    public string Type { get; set; }

    #region AutoComplete

    [Parameter]
    public string AutoComplete { get; set; }

    protected virtual string GetAutoComplete() => AutoComplete;

    #endregion AutoComplete

    [Parameter]
    public bool IsReadOnly { get; set; }

    [Parameter]
    public IList<string> DataList { get; set; }
}
