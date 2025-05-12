using System.Windows.Media;

namespace Shipwreck.ViewModelUtils.Controls;

public static class DependencyObjectHelper
{
    public static void EndEdit(this DependencyObject element)
    {
        for (var elem = FocusManager.GetFocusedElement(element) as DependencyObject;
            elem != null;
            elem = VisualTreeHelper.GetParent(elem))
        {
            var en = elem.GetLocalValueEnumerator();
            while (en.MoveNext())
            {
                var e = en.Current;
                if (BindingOperations.IsDataBound(elem, e.Property)
                    && BindingOperations.GetBindingExpression(elem, e.Property) is BindingExpression be)
                {
                    be.UpdateSource();
                }
            }
        }
    }
}
