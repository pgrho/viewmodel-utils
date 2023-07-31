namespace Shipwreck.ViewModelUtils.Searching;

public interface IFrameworkSearchPageViewModel : ISortablePageViewModel, ISelectablesHost
{
    BulkUpdateableCollection<SearchPropertyViewModel> Properties { get; }

    SearchPropertyViewModel ResolveParent(SearchPropertyViewModel property)
#if NET7_0_OR_GREATER
    {
        var li = property.Name?.LastIndexOf('.');
        if (li >= 0)
        {
            var pn = property.Name.Substring(0, li.Value);
            return Properties.FirstOrDefault(e => e.Name == pn);
        }
        return null;
    }
#else
    ;
#endif

    BulkUpdateableCollection<ConditionViewModel> Conditions { get; }

    string GetPropertyDisplayName(string property);

    ConditionViewModel CreateCondition(SearchPropertyViewModel property);

    ConditionViewModel GetCondition(string property);

    ConditionViewModel GetOrCreateCondition(string property);

    ConditionViewModel CreateOrGetCondition(string property);

    int? PageCount { get; }

    #region Commands

    CommandViewModelBase SearchCommand { get; }
    CommandViewModelCollection SearchSubCommands { get; }
    CommandViewModelCollection SelectionCommands { get; }
    bool IsSearching { get; }

    void BeginSearch();

    CommandViewModelBase AddConditionsCommand { get; }

    #endregion Commands

    string CurrentQuery { get; }

    string GetQueryString();

    ReadOnlyCollection<SortDescription> CurrentOrder { get; }
    int? CurrentPageIndex { get; }
    int? CurrentPageNumber { get; }
    int? CurrentPageSize { get; }

    bool SetParameter(string query, string order, int? pageIndex, int? pageSize);

    IEnumerable<CommandViewModelBase> CreateConditionCommands(ConditionViewModel condition)
#if NET5_0_OR_GREATER
            => null
#endif
            ;
}
