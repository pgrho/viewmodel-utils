namespace Shipwreck.ViewModelUtils;

public static class ColumnVisibilityHelper
{
    private static class EnumInfo<T>
        where T : struct, Enum, IConvertible
    {
        internal static readonly T[] Flags;

        static EnumInfo()
        {
            Flags = Enum.GetValues(typeof(T)).Cast<T>().Where(e => GetPopCount((ulong)e.ToInt64(null)) == 1).ToArray();
        }
    }

    private static class ColumnVisibilityInfo<T>
        where T : struct, Enum, IConvertible
    {
        private static readonly Dictionary<string, (long, long, long)> _Masks = new Dictionary<string, (long, long, long)>();

        internal static (long available, long @default, long required) GetMasks(string contextName)
        {
            contextName ??= string.Empty;
            lock (_Masks)
            {
                if (!_Masks.TryGetValue(contextName, out var tp))
                {
                    var a = -1L;
                    var d = 0L;
                    var r = 0L;

                    foreach (var f in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public))
                    {
                        var v = (T)f.GetValue(null);
                        var lv = v.ToInt64(null);

                        var cv = f.GetCustomAttributes<ColumnVisibilityAttribute>().FirstOrDefault(e => e.ContextName == contextName)?.Visibility ?? default;

                        var vis = (cv & ColumnVisibility.Visible) != 0;
                        var locked = (cv & ColumnVisibility.Locked) != 0;

                        if (vis)
                        {
                            d |= lv;

                            if (locked)
                            {
                                r |= lv;
                            }
                        }

                        if (vis || !locked)
                        {
                            a |= lv;
                        }
                    }
                    var ut = typeof(T).GetEnumUnderlyingType();
                    tp = (a, d, r);

                    _Masks[contextName] = tp;
                }
                return tp;
            }
        }
    }

    public static IEnumerable<T> GetFlags<T>()
        where T : struct, Enum, IConvertible
        => EnumInfo<T>.Flags;

    public static T GetAvailableColumns<T>()
        where T : struct, Enum, IConvertible
        => GetAvailableColumns<T>(null);

    public static T GetDefaultColumns<T>()
        where T : struct, Enum, IConvertible
        => GetDefaultColumns<T>(null);

    public static T GetRequiredColumns<T>()
        where T : struct, Enum, IConvertible
        => GetRequiredColumns<T>(null);

    public static T GetAvailableColumns<T>(string contextName)
        where T : struct, Enum, IConvertible
        => (T)Enum.ToObject(typeof(T), ColumnVisibilityInfo<T>.GetMasks(contextName).available);

    public static T GetDefaultColumns<T>(string contextName)
        where T : struct, Enum, IConvertible
        => (T)Enum.ToObject(typeof(T), ColumnVisibilityInfo<T>.GetMasks(contextName).@default);

    public static T GetRequiredColumns<T>(string contextName)
        where T : struct, Enum, IConvertible
        => (T)Enum.ToObject(typeof(T), ColumnVisibilityInfo<T>.GetMasks(contextName).required);

    public static byte GetPopCount(ulong v)
    {
        unchecked
        {
#if USE_INTRINSIC
                if (Popcnt.X64.IsSupported)
                {
                    return (byte)Popcnt.X64.PopCount(v);
                }
                if (Popcnt.IsSupported)
                {
                    return (byte)(Popcnt.PopCount((uint)v) + Popcnt.PopCount((uint)(v >> 32)));
                }
#endif
            v = v - ((v >> 1) & 0x5555555555555555UL);
            v = (v & 0x3333333333333333UL) + ((v >> 2) & 0x3333333333333333UL);
            return (byte)((((v + (v >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
        }
    }
}
