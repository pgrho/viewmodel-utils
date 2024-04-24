namespace Shipwreck.ViewModelUtils.Searching;

public interface IFrameworkSearchPageViewModel : ISortablePageViewModel, ISelectablesHost, ISearchPropertiesHost
{
    int? PageCount { get; }

    #region Commands

    CommandViewModelBase SearchCommand { get; }
    CommandViewModelCollection SearchSubCommands { get; }
    CommandViewModelCollection SelectionCommands { get; }

    #endregion Commands

    string CurrentQuery { get; }

    string GetQueryString();

    ReadOnlyCollection<SortDescription> CurrentOrder { get; }
    int? CurrentPageIndex { get; }
    int? CurrentPageNumber { get; }
    int? CurrentPageSize { get; }

    bool SetParameter(string query, string order, int? pageIndex, int? pageSize);
}
