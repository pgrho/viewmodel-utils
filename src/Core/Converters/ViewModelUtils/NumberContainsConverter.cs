using System;
using System.Globalization;

namespace Shipwreck.ViewModelUtils
{
    public sealed class NumberContainsConverter : BooleanConverterBase
    {
        public double Lowerbound { get; set; } = double.MinValue;
        public bool ContainsLowerbound { get; set; }
        public double Upperbound { get; set; } = double.MaxValue;
        public bool ContainsUpperbound { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IConvertible c)
            {
                var d = c.ToDouble(culture);
                var lc = Lowerbound.CompareTo(d);
                var uc = d.CompareTo(Upperbound);

                return ToResult(
                    (lc < 0 || (lc == 0 && ContainsLowerbound))
                    && (uc < 0 || (uc == 0 && ContainsUpperbound)),
                    targetType,
                    culture);
            }
            return ToResult(false, targetType, culture);
        }
    }
}
