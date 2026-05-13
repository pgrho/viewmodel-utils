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

    #region NullableDateTime

    private DateTime? _NullableDateTime;

    private DateTime _DateTime; 

#pragma warning disable BL0005
#pragma warning disable BL0007

    [Parameter]
    public DateTime? NullableDateTime
    {
        get => DateTimePicker?.Value ?? _NullableDateTime;
        set
        {
            if (DateTimePicker is DateTimePicker p)
            {
                p.Value = value;
            }
            else
            {
                _NullableDateTime = value;
            }
        }
    }

    [Parameter]
    public DateTime DateTime
    {
        get => DateTimePicker?.NonNullValue ?? _DateTime;
        set
        {
            if (DateTimePicker is DateTimePicker p)
            {
                p.NonNullValue = value;
            }
            else
            {
                _DateTime = value;
            }
        }
    }

#pragma warning restore BL0007
#pragma warning restore BL0005

    [Parameter]
    public Action<DateTime?> NullableDateTimeChanged { get; set; }

    [Parameter]
    public Action<DateTime> DateTimeChanged { get; set; }

    #endregion NullableDateTime
}
