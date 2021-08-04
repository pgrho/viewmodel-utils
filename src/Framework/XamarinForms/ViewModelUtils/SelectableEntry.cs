
using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils
{
    public class SelectableEntry : Entry
    {
        public static readonly BindableProperty SelectAllOnFocusProperty
            = BindableProperty.Create(
                nameof(SelectAllOnFocus), typeof(bool), typeof(SelectableEntry),
                defaultValue: true);

        public bool SelectAllOnFocus
        {
            get => (bool)GetValue(SelectAllOnFocusProperty);
            set => SetValue(SelectAllOnFocusProperty, value);
        }
    }
}
