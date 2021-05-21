using System.Windows;
using System.Windows.Data;

namespace Shipwreck.ViewModelUtils.Controls
{
    public static class BindingHelper
    {
        private static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(object), typeof(BindingHelper));

        public static object ResolveValue(this BindingBase b, object item)
        {
            var d = new FrameworkContentElement() { DataContext = item };
            BindingOperations.SetBinding(d, ValueProperty, b);
            return d.GetValue(ValueProperty);
        }
    }
}
