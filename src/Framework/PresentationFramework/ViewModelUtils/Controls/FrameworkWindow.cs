#if NET5_0
using Notifications.Wpf.Core.Controls;
#else

using Notifications.Wpf.Controls;

#endif

namespace Shipwreck.ViewModelUtils.Controls;

public class FrameworkWindow : MetroWindow
{
    public FrameworkWindow()
    {
        Loaded += FrameworkWindow_Loaded;
        DataContextChanged += FrameworkWindow_DataContextChanged;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild("PART_MetroActiveDialogContainer") is FrameworkElement dg && dg.Parent is Grid pg)
        {
            _NotificationArea = new FrameworkNotificationArea()
            {
                MaxItems = 3,
                Position = NotificationPosition.BottomRight,
                Name = "GeneratedNotificationArea_" + GetHashCode()
            };
            Grid.SetRow(_NotificationArea, Grid.GetRow(dg));
            Grid.SetColumn(_NotificationArea, Grid.GetColumn(dg));
            Grid.SetRowSpan(_NotificationArea, Grid.GetRowSpan(dg));
            Grid.SetColumnSpan(_NotificationArea, Grid.GetColumnSpan(dg));
            Panel.SetZIndex(_NotificationArea, Panel.GetZIndex(dg) + 1);

            pg.Children.Add(_NotificationArea);
        }
        else
        {
            _NotificationArea = null;
        }
    }

    private FrameworkNotificationArea _NotificationArea;

    protected internal virtual FrameworkNotificationArea GetNotificationArea()
        => _NotificationArea;

    private void FrameworkWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        => (DataContext as WindowViewModel)?.OnLoaded();

    private void FrameworkWindow_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
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

    private void RequestFocus_RequestFocus(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        => FocusRequested(e.PropertyName);

    protected virtual void FocusRequested(string propertyName)
    {
    }

    protected override void OnClosed(EventArgs e)
    {
        (DataContext as IDisposable)?.Dispose();
        base.OnClosed(e);
    }
}
