namespace Shipwreck.ViewModelUtils;

public interface IPaginatable : INotifyPropertyChanged
{
    int? TotalCount { get; }
    int? PageIndex { get; }
    int? PageSize { get; }

    ReadOnlyCollection<SortDescription> Order { get; }

    void ToggleSortKey(string member);

    void NavigateTo(int pageIndex);

    void SetPageSize(int value);
}
