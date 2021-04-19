#if IS_WPF

using System.Windows;
using System.Windows.Controls;

#else
using Xamarin.Forms;
#endif

namespace Shipwreck.ViewModelUtils.Controls
{
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
    }
}
