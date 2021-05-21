using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Shipwreck.ViewModelUtils.Client
{
    public sealed class ExtensionDataConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s)
            {
                Dictionary<string, string> d = null;
                ExtensibleEntityHelper.Enumerate(s, ref d);
                return d?.Count > 0 ? new ExtensionData(d) : null;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            => destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                string s = null;
                return ExtensibleEntityHelper.GetString(ref s, (ExtensionData)value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
