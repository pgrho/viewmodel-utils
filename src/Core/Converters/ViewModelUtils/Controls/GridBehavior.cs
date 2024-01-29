namespace Shipwreck.ViewModelUtils.Controls;

public static class GridBehavior
{
#if IS_WPF

    public static readonly DependencyProperty ColumnDefinitionsProperty
        = DependencyProperty.RegisterAttached(
            "ColumnDefinitions", typeof(GridLength[]), typeof(GridBehavior),
            new FrameworkPropertyMetadata(null, (d, e) => OnColumnDefinitionsChanged(d as Grid, e.OldValue, e.NewValue)));

    public static readonly DependencyProperty RowDefinitionsProperty
        = DependencyProperty.RegisterAttached(
            "RowDefinitions", typeof(GridLength[]), typeof(GridBehavior),
            new FrameworkPropertyMetadata(null, (d, e) => OnRowDefinitionsChanged(d as Grid, e.OldValue, e.NewValue)));

#else

    public static readonly BindableProperty ColumnDefinitionsProperty
        = BindableProperty.CreateAttached(
            "ColumnDefinitions", typeof(GridLength[]), typeof(GridBehavior), null,
            propertyChanged: (b, o, n) => OnColumnDefinitionsChanged(b as Grid, o, n));

    public static readonly BindableProperty RowDefinitionsProperty
        = BindableProperty.CreateAttached(
            "RowDefinitions", typeof(GridLength[]), typeof(GridBehavior), null,
            propertyChanged: (b, o, n) => OnRowDefinitionsChanged(b as Grid, o, n));

#endif

    #region ColumnDefinitions

    public static GridLength[] GetColumnDefinitions(Grid obj)
        => (GridLength[])obj.GetValue(ColumnDefinitionsProperty);

    public static void SetColumnDefinitions(Grid obj, GridLength[] value)
        => obj.SetValue(ColumnDefinitionsProperty, value);

    private static void OnColumnDefinitionsChanged(Grid g, object oldValue, object newValue)
    {
        if (g != null)
        {
            var v = newValue as GridLength[];
            if (v?.Length > 0)
            {
                g.ColumnDefinitions.Clear();
                foreach (var l in v)
                {
                    g.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = l
                    });
                }
            }
            else
            {
                g.ColumnDefinitions.Clear();
            }
        }
    }

    #endregion ColumnDefinitions

    #region RowDefinitions

    public static GridLength[] GetRowDefinitions(Grid obj)
        => (GridLength[])obj.GetValue(RowDefinitionsProperty);

    public static void SetRowDefinitions(Grid obj, GridLength[] value)
        => obj.SetValue(RowDefinitionsProperty, value);

    private static void OnRowDefinitionsChanged(Grid g, object oldValue, object newValue)
    {
        if (g != null)
        {
            var v = newValue as GridLength[];
            if (v?.Length > 0)
            {
                g.RowDefinitions.Clear();
                foreach (var l in v)
                {
                    g.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = l
                    });
                }
            }
            else
            {
                g.RowDefinitions.Clear();
            }
        }
    }

    #endregion RowDefinitions

#if IS_XAMARIN_FORMS

    #region WrapIndex

    public static readonly BindableProperty WrapIndexProperty
    = BindableProperty.CreateAttached(
        "WrapIndex", typeof(int?), typeof(ViewHelper), null,
        propertyChanged: OnWrapIndexChanged);

    public static int? GetWrapIndex(View obj)
    => (int?)obj.GetValue(WrapIndexProperty);

    public static void SetWrapIndex(View obj, int? value)
        => obj.SetValue(WrapIndexProperty, value);

    private static void OnWrapIndexChanged(BindableObject g, object oldValue, object newValue)
    {
        var v = g as View;
        if (v != null)
        {
            v.PropertyChanged -= V_PropertyChanged;
            if (GetWrapIndex(v) is int i
                && i >= 0)
            {
                v.PropertyChanged += V_PropertyChanged;

                if (v.Parent is Grid p)
                {
                    if (p.ColumnDefinitions.Count is int c
                        && c > 0)
                    {
                        Grid.SetColumn(g, i % c);
                        Grid.SetRow(g, i / c);
                        return;
                    }
                }
            }
            Grid.SetColumn(g, 0);
            Grid.SetRow(g, 0);
        }
    }

    private static void V_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(View.Parent):
                OnWrapIndexChanged((BindableObject)sender, null, null);
                break;
        }
    }

    #endregion WrapIndex

#endif
}
