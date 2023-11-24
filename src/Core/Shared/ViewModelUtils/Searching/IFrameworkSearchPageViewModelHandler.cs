namespace Shipwreck.ViewModelUtils.Searching;

public interface IFrameworkSearchPageViewModelHandler : IFrameworkPageViewModelHandler
{
    IEnumerable<SearchPropertyGroupViewModel> CreatePropertyGroups(IFrameworkSearchPageViewModel page);
}
