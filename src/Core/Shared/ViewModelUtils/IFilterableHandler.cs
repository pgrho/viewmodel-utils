namespace Shipwreck.ViewModelUtils;

public interface IFilterableHandler
{
    IEnumerable<FilterOption> GetFilterOptions(IFilterable filterable, string key)
#if NET9_0_OR_GREATER
        => null
#endif
        ;
}
