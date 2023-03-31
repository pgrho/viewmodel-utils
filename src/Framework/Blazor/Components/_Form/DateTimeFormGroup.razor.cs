namespace Shipwreck.ViewModelUtils.Components;

public partial class DateTimeFormGroup
{
    #region DateTimePickerId

    private string _DateTimePickerId;

    [Parameter]
    public string DateTimePickerId
    {
        get => _DateTimePickerId ??= (FormGroupId + "--datetimepicker");
        set => SetProperty(ref _DateTimePickerId, value);
    }

    #endregion DateTimePickerId

    [Parameter]
    public bool IsReadOnly { get; set; }

    [Parameter]
    public DateTimePickerMode Mode { get; set; } = DateTimePickerMode.Date;

    [Parameter]
    public bool UseCurrent { get; set; }

    #region NullableDateTime

    private DateTime? _NullableDateTime;

    [Parameter]
    public DateTime? NullableDateTime
    {
        get => _NullableDateTime;
        set
        {
            if (!Equals(value, _NullableDateTime))
            {
                _NullableDateTime = value;
                using (Host?.PushPropertyChangedExpectation(BindingPropertyName))
                {
                    NullableDateTimeChanged?.Invoke(_NullableDateTime);
                }
                using (Host?.PushPropertyChangedExpectation(BindingPropertyName))
                {
                    DateTimeChanged?.Invoke(DateTime);
                }
                if (!IsUpdatingSource)
                {
                    ShouldRenderCore = true;
                }
            }
        }
    }

    [Parameter]
    public DateTime DateTime
    {
        get => _NullableDateTime ?? default;
        set => NullableDateTime = value;
    }

    protected DateTime? InternalValue
    {
        get => NullableDateTime;
        set
        {
            IsUpdatingSource = true;
            NullableDateTime = value;
            IsUpdatingSource = false;
        }
    }

    [Parameter]
    public Action<DateTime> DateTimeChanged { get; set; }

    [Parameter]
    public Action<DateTime?> NullableDateTimeChanged { get; set; }

    [Parameter]
    public string BindingPropertyName { get; set; }

    #endregion NullableDateTime
}
