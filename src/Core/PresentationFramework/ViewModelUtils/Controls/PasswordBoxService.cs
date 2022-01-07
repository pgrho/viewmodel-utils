namespace Shipwreck.ViewModelUtils.Controls;

public static class PasswordBoxService
{
    public static readonly DependencyProperty HandlePasswordChangedProperty
       = DependencyProperty.RegisterAttached("HandlePasswordChanged", typeof(bool?), typeof(PasswordBoxService), new FrameworkPropertyMetadata(null, (d, e) =>
       {
           if (d is PasswordBox pb)
           {
               pb.RemoveHandler(PasswordBox.PasswordChangedEvent, (RoutedEventHandler)Pb_PasswordChanged);

               if (e.NewValue is bool b && b)
               {
                   pb.AddHandler(PasswordBox.PasswordChangedEvent, (RoutedEventHandler)Pb_PasswordChanged);
               }
           }
       }));

    public static readonly DependencyProperty PasswordProperty
        = DependencyProperty.RegisterAttached(
            "Password", typeof(string), typeof(PasswordBoxService), new FrameworkPropertyMetadata(null, OnPasswordChanged)
            { BindsTwoWayByDefault = true });

    public static bool? GetHandlePasswordChanged(PasswordBox obj)
        => (bool?)obj.GetValue(HandlePasswordChangedProperty);

    public static void SetHandlePasswordChanged(PasswordBox obj, bool? value)
        => obj.SetValue(HandlePasswordChangedProperty, value);

    public static string GetPassword(PasswordBox obj)
        => (string)obj.GetValue(PasswordProperty);

    public static void SetPassword(PasswordBox obj, string value)
        => obj.SetValue(PasswordProperty, value);

    private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PasswordBox pb)
        {
            SetHandlePasswordChanged(pb, GetHandlePasswordChanged(pb) ?? true);

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
