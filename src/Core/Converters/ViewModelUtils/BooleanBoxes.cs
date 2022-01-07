namespace Shipwreck.ViewModelUtils;

public static class BooleanBoxes
{
    public static object True { get; } = true;
    public static object False { get; } = false;

    public static ReadOnlyCollection<object> TrueFalse { get; }
        = Array.AsReadOnly(new[] { True, False });

    public static ReadOnlyCollection<object> FalseTrue { get; }
        = Array.AsReadOnly(new[] { False, True });

    public static ReadOnlyCollection<object> NullTrueFalse { get; }
        = Array.AsReadOnly(new[] { null, True, False });

    public static ReadOnlyCollection<object> NullFalseTrue { get; }
        = Array.AsReadOnly(new[] { null, False, True });
}
