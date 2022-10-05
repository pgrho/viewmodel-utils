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

    #region Type

    private string _Type;

    [Parameter]
    public string Type
    {
        get => _Type;
        set => SetProperty(ref _Type, value);
    }

    #endregion Type

    #region AutoComplete

    private string _AutoComplete;

    [Parameter]
    public string AutoComplete
    {
        get => _AutoComplete;
        set => SetProperty(ref _AutoComplete, value);
    }

    protected virtual string GetAutoComplete() => AutoComplete;

    #endregion AutoComplete

    #region IsReadOnly

    private bool _IsReadOnly;

    [Parameter]
    public bool IsReadOnly
    {
        get => _IsReadOnly;
        set => SetProperty(ref _IsReadOnly, value);
    }

    #endregion IsReadOnly

    #region DataList

    private ReadOnlyCollection<string> _DataList;

    [Parameter]
    public IList<string> DataList
    {
        get => _DataList;
        set
        {
            var vs = value?.Count > 0 ? Array.AsReadOnly(value.ToArray()) : null;

            if (vs == null
                || _DataList == null
                || !vs.SequenceEqual(_DataList))
            {
                SetProperty(ref _DataList, vs);
            }
        }
    }

    #endregion DataList
}
