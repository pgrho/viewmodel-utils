namespace Shipwreck.ViewModelUtils;

public interface IMemberFilter<T>
{
    string? Filter { get; set; }

    bool IsMatch(T item);
}
