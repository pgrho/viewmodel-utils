using System.Windows;
using System.Windows.Controls;

namespace Shipwreck.ViewModelUtils.Controls
{
    public class EntitySelectorView : Control
    {
        static EntitySelectorView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EntitySelectorView), new FrameworkPropertyMetadata(typeof(EntitySelectorView)));
        }

        public static readonly DependencyProperty ContentTemplateProperty
            = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(EntitySelectorView), new FrameworkPropertyMetadata(null));

        public DataTemplate ContentTemplate
        {
            get => (DataTemplate)GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }
    }
}
