namespace Shipwreck.ViewModelUtils.Searching;

[Flags]
public enum ConditionCreationBehavior
{
    None = 0,
    CreateNew = 1,
    PreferNew = 2,
    PreferLast = 4,
}
