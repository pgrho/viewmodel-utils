using System.Windows.Markup;

namespace Shipwreck.ViewModelUtils.Controls;

[ContentProperty(nameof(Templates))]
public class DataTemplateSelector : System.Windows.Controls.DataTemplateSelector
{
    private readonly Dictionary<Type, DataTemplate> _Templates = new Dictionary<Type, DataTemplate>();

    public Dictionary<Type, DataTemplate> Templates
    {
        get => _Templates;
        set
        {
            if (value != _Templates)
            {
                _Templates.Clear();
                if (value != null)
                {
                    foreach (var kv in value)
                    {
                        _Templates.Add(kv.Key, kv.Value);
                    }
                }
            }
        }
    }

    public System.Windows.Controls.DataTemplateSelector BasedOn { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        for (var t = item?.GetType(); t != null; t = t.BaseType)
        {
            if (Templates.TryGetValue(t, out var tp))
            {
                return tp;
            }
        }

        return BasedOn?.SelectTemplate(item, container)
            ?? (item is BindingProxy bp
                && bp.Data != null
                && bp.Data != bp ? SelectTemplate(bp.Data, container) : null)
            ?? base.SelectTemplate(item, container);
    }
}
