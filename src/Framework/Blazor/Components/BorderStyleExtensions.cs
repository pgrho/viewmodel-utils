namespace Shipwreck.ViewModelUtils.Components;

public static class BorderStyleExtensions
{
    private static readonly (BorderStyle style, string name)[] _ClassNames = {
            (BorderStyle.OutlinePrimary, "outline-primary"),
            (BorderStyle.OutlineSecondary, "outline-secondary"),
            (BorderStyle.OutlineDanger, "outline-danger"),
            (BorderStyle.OutlineWarning, "outline-warning"),
            (BorderStyle.OutlineSuccess, "outline-success"),
            (BorderStyle.OutlineInfo, "outline-info"),
            (BorderStyle.Primary, "primary"),
            (BorderStyle.Secondary, "secondary"),
            (BorderStyle.Danger, "danger"),
            (BorderStyle.Warning, "warning"),
            (BorderStyle.Success, "success"),
            (BorderStyle.Info, "info"),
            (BorderStyle.Link, "link"),
        };

    public static string ToClassName(this BorderStyle t, string prefix)
    {
        foreach (var cn in _ClassNames)
        {
            if ((t & cn.style) == cn.style)
            {
                return prefix + cn.name;
            }
        }
        return null;
    }

    public static string ToButtonClass(this BorderStyle t, BorderStyle defaultType = BorderStyle.None)
        => t.ToClassName("btn-") ?? defaultType.ToClassName("btn-");

    public static string ToBadgeClass(this BorderStyle t, BorderStyle defaultType = BorderStyle.None)
        => (t & ~BorderStyle.Outline).ToClassName("badge-")
        ?? defaultType.ToClassName("badge-");

    public static string ToTableClass(this BorderStyle t, BorderStyle defaultType = BorderStyle.None)
        => (t & ~BorderStyle.Outline).ToClassName("table-")
        ?? defaultType.ToClassName("table-");

    public static string ToTextClass(this BorderStyle t)
        => (t & ~BorderStyle.Outline).ToClassName("text-");

    public static BorderStyle IgnoreDefault(this BorderStyle t, BorderStyle @default = BorderStyle.OutlineSecondary)
        => t == @default ? BorderStyle.None : t;
}
