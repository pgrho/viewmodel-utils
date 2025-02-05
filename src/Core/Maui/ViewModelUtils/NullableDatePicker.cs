using Microsoft.Maui.Graphics;

namespace Shipwreck.ViewModelUtils;

public class NullableDatePicker : DatePicker
{
    public static readonly BindableProperty NullableDateProperty
        = BindableProperty.Create(
            nameof(NullableDate),
            typeof(DateTime?),
            typeof(NullableDatePicker),
            defaultBindingMode: BindingMode.TwoWay, propertyChanged: (d, o, n) =>
            {
                if (d is NullableDatePicker cdp)
                {
                    cdp?.UpdateDate();
                }
            });

    public NullableDatePicker()
    {
        Format = "----/--/--";
    }

    public DateTime? NullableDate
    {
        get => (DateTime?)GetValue(NullableDateProperty);
        set => SetValue(NullableDateProperty, value);
    }

    private void UpdateDate()
    {
        if (NullableDate != null)
        {
            TextColor = Colors.Black;
            Format = "yyyy/MM/dd";
            Date = NullableDate.Value;
        }
        else
        {
            TextColor = Color.FromRgb(0x6c, 0x75, 0x7d);
            Format = "----/--/--";
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        UpdateDate();
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(Date))
        {
            NullableDate = Date;
        }
    }
}
