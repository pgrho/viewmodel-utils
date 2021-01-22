namespace Shipwreck.ViewModelUtils
{
    public enum BorderStyle
    {
        None = 0,
        Outline = 1,
        Primary = 1 << 1,
        Secondary = 1 << 2,
        Danger = 1 << 3,
        Warning = 1 << 4,
        Success = 1 << 5,
        Info = 1 << 6,
        Link = 1 << 7,

        OutlinePrimary = Outline | Primary,
        OutlineSecondary = Outline | Secondary,
        OutlineDanger = Outline | Danger,
        OutlineWarning = Outline | Warning,
        OutlineSuccess = Outline | Success,
        OutlineInfo = Outline | Info,
    }
}
