#if !NET7_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
internal sealed class DynamicDependencyAttribute : Attribute
{
    public DynamicDependencyAttribute(string memberSignature) { }
}
#endif
