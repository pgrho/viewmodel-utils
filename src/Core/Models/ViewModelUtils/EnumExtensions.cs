using System;
using System.Collections.Generic;
using System.Reflection;

namespace Shipwreck.ViewModelUtils
{
    public static partial class EnumExtensions
    {
        private static class EnumInfo<T>
            where T : struct, Enum, IConvertible
        {
            internal static readonly Dictionary<T, string> Icons;

            static EnumInfo()
            {
                Icons = new Dictionary<T, string>();
                foreach (var f in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var v = (T)f.GetValue(null);

                    var icon = f.GetCustomAttribute<IconAttribute>()?.Icon;
                    if (!string.IsNullOrEmpty(icon))
                    {
                        Icons[v] = icon;
                    }
                }
            }
        }

        public static string GetIcon<T>(this T value)
            where T : struct, Enum, IConvertible
            => EnumInfo<T>.Icons.TryGetValue(value, out var s) ? s : null;
    }
}
