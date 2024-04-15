namespace Shipwreck.ViewModelUtils;

public partial interface IFilterable : IPaginatable
{
    bool IsFilterSupported(string key);

    bool HasFiltered();

    string? GetFilter(string key);

    void SetFilter(string key, string? value);

    string? GetFilterName(string key);

    string? GetFilterDescription(string key);

    IEnumerable<FilterOption>? GetFilterOptions(string key);

    void ClearFilter();
}
