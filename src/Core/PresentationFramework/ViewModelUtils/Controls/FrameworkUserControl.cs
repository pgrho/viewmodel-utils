namespace Shipwreck.ViewModelUtils.Controls;

public class FrameworkUserControl : UserControl
{
    public FrameworkUserControl()
    {
        DataContextChanged += FrameworkUserControl_DataContextChanged;
        Unloaded += FrameworkUserControl_Unloaded;
    }

    private void FrameworkUserControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is IRequestFocus of)
        {
            of.RequestFocus -= RequestFocus_RequestFocus;
        }
        if (e.NewValue is IRequestFocus f)
        {
            f.RequestFocus -= RequestFocus_RequestFocus;
            f.RequestFocus += RequestFocus_RequestFocus;
        }
    }

    private void FrameworkUserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (!e.Handled)
        {
            (DataContext as IDisposable)?.Dispose();
        }
    }

    private void RequestFocus_RequestFocus(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        => FocusRequested(e.PropertyName);

    protected virtual void FocusRequested(string propertyName)
    {
    }
}
