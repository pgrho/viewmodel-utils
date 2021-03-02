using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;

namespace Shipwreck.ViewModelUtils.Controls
{
    [ContentProperty(nameof(Styles))]
    public class StyleSelector : System.Windows.Controls.StyleSelector
    {
        private readonly Dictionary<Type, Style> _Styles = new Dictionary<Type, Style>();

        public Dictionary<Type, Style> Styles
        {
            get => _Styles;
            set
            {
                if (value != _Styles)
                {
                    _Styles.Clear();
                    if (value != null)
                    {
                        foreach (var kv in value)
                        {
                            _Styles.Add(kv.Key, kv.Value);
                        }
                    }
                }
            }
        }

        public Style DefaultStyle { get; set; }

        public System.Windows.Controls.StyleSelector BasedOn { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            for (var t = item?.GetType(); t != null; t = t.BaseType)
            {
                if (Styles.TryGetValue(t, out var tp))
                {
                    return tp;
                }
            }

            return BasedOn?.SelectStyle(item, container) ?? DefaultStyle ?? base.SelectStyle(item, container);
        }
    }
}
