#if !NET7_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis;

internal sealed class RequiresUnreferencedCodeAttribute : Attribute
{
    public RequiresUnreferencedCodeAttribute(string message) { }
}
#endif
