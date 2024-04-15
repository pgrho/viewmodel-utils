namespace Shipwreck.ViewModelUtils;

public interface IMemberFilter
{
    string? Name { get; }
    string? Description { get; }

    string? Filter { get; set; }
    bool IsMatch(object obj);
}
public interface IMemberFilter<T> : IMemberFilter
{
#if NET7_0_OR_GREATER
    bool IMemberFilter.IsMatch(object obj) => obj is T item && IsMatch(item);
#endif

    bool IsMatch(T item);
}
