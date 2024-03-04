using Microsoft.Maui.Controls.Compatibility;

namespace Shipwreck.ViewModelUtils;

public sealed class ItemIndexConverter : IValueConverter
{
    [DefaultValue(true)]
    public bool VisibleOnly { get; set; } = true;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is View v)
        {
            if (v.Parent is Layout<View> l)
            {
                if (VisibleOnly)
                {
                    var i = -1;
                    foreach (var e in l.Children)
                    {
                        if (e.IsVisible)
                        {
                            i++;
                            return i;
                        }
                    }
                    return -1;
                }
                return l.Children.IndexOf(v);
            }
        }
        return -1;
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
