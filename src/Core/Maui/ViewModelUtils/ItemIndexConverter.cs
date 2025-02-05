namespace Shipwreck.ViewModelUtils;

public sealed class ItemIndexConverter : IValueConverter
{
    [DefaultValue(true)]
    public bool VisibleOnly { get; set; } = true;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is View v)
        {
            if (v.Parent is Layout l)
            {
                if (VisibleOnly)
                {
                    return l.Children
                            .Where(e => e.Visibility == Visibility.Visible)
                            .ToList()
                            .IndexOf(v);
                }
                return l.Children.IndexOf(v);
            }
        }
        return -1;
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
