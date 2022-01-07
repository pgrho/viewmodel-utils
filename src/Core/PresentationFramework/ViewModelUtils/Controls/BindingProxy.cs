namespace Shipwreck.ViewModelUtils.Controls;

public sealed class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty
        = DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new FrameworkPropertyMetadata(null));

    public object Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public static readonly DependencyProperty DisplayNameProperty
      = DependencyProperty.Register(nameof(DisplayName), typeof(string), typeof(BindingProxy), new PropertyMetadata(null));

    public string DisplayName
    {
        get { return (string)GetValue(DisplayNameProperty); }
        set { SetValue(DisplayNameProperty, value); }
    }

    protected override Freezable CreateInstanceCore()
        => new BindingProxy();
}
