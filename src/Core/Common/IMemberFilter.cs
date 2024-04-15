namespace Shipwreck.ViewModelUtils;

public interface IMemberFilter<T>
{
    string Name { get; }
    string Description { get; }

    string? Filter { get; set; }

    bool IsMatch(T item);
}
