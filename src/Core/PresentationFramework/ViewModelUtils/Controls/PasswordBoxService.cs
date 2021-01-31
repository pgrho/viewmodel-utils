using System.Windows;
using System.Windows.Controls;

namespace Shipwreck.ViewModelUtils.Controls
{
    public static class PasswordBoxService
    {
        public static readonly DependencyProperty PasswordProperty
            = DependencyProperty.RegisterAttached(
                "Password", typeof(string), typeof(PasswordBoxService), new FrameworkPropertyMetadata(null, OnPasswordChanged)
                { BindsTwoWayByDefault = true });

        public static string GetPassword(PasswordBox obj)
            => (string)obj.GetValue(PasswordProperty);

        public static void SetPassword(PasswordBox obj, string value)
            => obj.SetValue(PasswordProperty, value);

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb)
            {
                pb.RemoveHandler(PasswordBox.PasswordChangedEvent, (RoutedEventHandler)Pb_PasswordChanged);
                pb.AddHandler(PasswordBox.PasswordChangedEvent, (RoutedEventHandler)Pb_PasswordChanged);
                if (e.NewValue is string s && s != pb.Password)
                {
                    pb.Password = s;
                }
            }
        }

        private static void Pb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
            {
                SetPassword(pb, pb.Password);
            }
        }
    }
}
