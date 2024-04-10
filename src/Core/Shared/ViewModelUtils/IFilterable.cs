namespace Shipwreck.ViewModelUtils;

public interface IFilterable : IPaginatable
{
    bool IsFilterSupported(string key);

    bool HasFiltered();

    string? GetFilter(string key);

    void SetFilter(string key, string? value);

    string? GetFilterName(string key);

    string? GetFilterDescription(string key);

    void ClearFilter();
}
