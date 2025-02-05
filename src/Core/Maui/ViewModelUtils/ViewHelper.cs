namespace Shipwreck.ViewModelUtils;

public static class ViewHelper
{
    public static readonly BindableProperty TapCommandProperty
    = BindableProperty.CreateAttached(
        "TapCommand", typeof(ICommand), typeof(ViewHelper), null,
        propertyChanged: OnTapCommandChanged);

    public static readonly BindableProperty TapCommandParameterProperty
        = BindableProperty.CreateAttached(
            "TapCommandParameter", typeof(object), typeof(ViewHelper), null,
            propertyChanged: OnTapCommandChanged);

    public static ICommand GetTapCommand(View obj)
    => (ICommand)obj.GetValue(TapCommandProperty);

    public static void SetTapCommand(View obj, ICommand value)
        => obj.SetValue(TapCommandProperty, value);

    public static object GetTapCommandParameter(View obj)
        => obj.GetValue(TapCommandParameterProperty);

    public static void SetTapCommandParameter(View obj, object value)
        => obj.SetValue(TapCommandParameterProperty, value);

    private static void OnTapCommandChanged(BindableObject g, object oldValue, object newValue)
    {
        if (g is View v)
        {
            var c = GetTapCommand(v);
            if (c == null)
            {
                for (var i = v.GestureRecognizers.Count - 1; i >= 0; i--)
                {
                    if (v.GestureRecognizers[i] is TapGestureRecognizer)
                    {
                        v.GestureRecognizers.RemoveAt(i);
                    }
                }
            }
            else
            {
                for (var i = 0; i < v.GestureRecognizers.Count; i++)
                {
                    if (v.GestureRecognizers[i] is TapGestureRecognizer tgr)
                    {
                        tgr.Command = c;
                        tgr.CommandParameter = GetTapCommandParameter(v);
                        return;
                    }
                }
                {
                    var tgr = new TapGestureRecognizer();
                    tgr.Command = c;
                    tgr.CommandParameter = GetTapCommandParameter(v);
                    v.GestureRecognizers.Add(tgr);
                }
            }
        }
    }
}
