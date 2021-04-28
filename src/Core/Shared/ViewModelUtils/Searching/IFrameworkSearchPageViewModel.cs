using System.Collections.ObjectModel;

namespace Shipwreck.ViewModelUtils.Searching
{
    public interface IFrameworkSearchPageViewModel : ISortablePageViewModel, ISelectablesHost
    {
        BulkUpdateableCollection<SearchPropertyViewModel> Properties { get; }

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
    }
}
