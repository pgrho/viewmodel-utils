using System;

namespace Shipwreck.ViewModelUtils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class IconAttribute : Attribute
    {
        public IconAttribute(string icon)
        {
            Icon = icon;
        }

        public string Icon { get; }
    }
}
