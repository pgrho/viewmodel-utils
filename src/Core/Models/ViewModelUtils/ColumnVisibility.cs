namespace Shipwreck.ViewModelUtils;

[Flags]
public enum ColumnVisibility
{
    Hidden,
    Visible = 1,
    Locked = 2,
}
