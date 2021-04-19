using System;

namespace Shipwreck.ViewModelUtils
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ColumnVisibilityAttribute : Attribute
    {
        public ColumnVisibilityAttribute(ColumnVisibility visibility)
            : this(null, visibility)
        {
        }

        public ColumnVisibilityAttribute(string contextName, ColumnVisibility visibility)
        {
            ContextName = contextName ?? string.Empty;
            Visibility = visibility;
        }

        public string ContextName { get; set; }

        public ColumnVisibility Visibility { get; set; }
    }
}
