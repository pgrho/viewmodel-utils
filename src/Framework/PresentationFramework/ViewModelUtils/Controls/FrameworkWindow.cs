using System;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace Shipwreck.ViewModelUtils.Controls
{
    public class FrameworkWindow : MetroWindow
    {
        public FrameworkWindow()
        {
            Loaded += FrameworkWindow_Loaded;
            DataContextChanged += FrameworkWindow_DataContextChanged;
        }

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
}
