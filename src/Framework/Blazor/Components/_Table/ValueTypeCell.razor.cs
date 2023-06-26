using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ViewModelUtils.Components;

partial class ValueTypeCell<T>
    where T : struct, IFormattable, IComparable, IComparable<T>, IEquatable<T>, IConvertible
{
    protected override string ToString(T value)
        => value.ToString("D", CultureInfo.InvariantCulture);

    protected override bool TryParse(string s, out T result)
    {
        try
        {
            result = (T)((IConvertible)s).ToType(typeof(T), CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
        }
        result = default;
        return false;
    }
}
