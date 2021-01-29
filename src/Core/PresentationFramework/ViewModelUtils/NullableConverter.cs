using System;
using System.Globalization;
using System.Windows.Data;

namespace Shipwreck.ViewModelUtils
{
    [ValueConversion(typeof(bool), typeof(string))]
    [ValueConversion(typeof(bool?), typeof(string))]
    [ValueConversion(typeof(sbyte), typeof(string))]
    [ValueConversion(typeof(sbyte?), typeof(string))]
    [ValueConversion(typeof(byte), typeof(string))]
    [ValueConversion(typeof(byte?), typeof(string))]
    [ValueConversion(typeof(short), typeof(string))]
    [ValueConversion(typeof(short?), typeof(string))]
    [ValueConversion(typeof(ushort), typeof(string))]
    [ValueConversion(typeof(ushort?), typeof(string))]
    [ValueConversion(typeof(int), typeof(string))]
    [ValueConversion(typeof(int?), typeof(string))]
    [ValueConversion(typeof(uint), typeof(string))]
    [ValueConversion(typeof(uint?), typeof(string))]
    [ValueConversion(typeof(long), typeof(string))]
    [ValueConversion(typeof(long?), typeof(string))]
    [ValueConversion(typeof(ulong), typeof(string))]
    [ValueConversion(typeof(ulong?), typeof(string))]
    [ValueConversion(typeof(float), typeof(string))]
    [ValueConversion(typeof(float?), typeof(string))]
    [ValueConversion(typeof(double), typeof(string))]
    [ValueConversion(typeof(double?), typeof(string))]
    [ValueConversion(typeof(decimal), typeof(string))]
    [ValueConversion(typeof(decimal?), typeof(string))]
    [ValueConversion(typeof(DateTime), typeof(string))]
    [ValueConversion(typeof(DateTime?), typeof(string))]
    //[ValueConversion(typeof(DateTimeOffset), typeof(string))]
    //[ValueConversion(typeof(DateTimeOffset?), typeof(string))]
    //[ValueConversion(typeof(TimeSpan), typeof(string))]
    //[ValueConversion(typeof(TimeSpan?), typeof(string))]
    public class NullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (value is IConvertible c)
            {
                return c.ToString(culture);
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value == null
                || (value is string s && string.IsNullOrWhiteSpace(s)))
                && Nullable.GetUnderlyingType(targetType) != null)
            {
                return null;
            }
            if (value is IConvertible c)
            {
                return c.ToType(Nullable.GetUnderlyingType(targetType) ?? targetType, culture);
            }
            throw new InvalidCastException();
        }
    }
}
