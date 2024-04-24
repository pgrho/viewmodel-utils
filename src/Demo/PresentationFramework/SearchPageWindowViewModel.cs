using System.Threading;
using Shipwreck.ViewModelUtils.Client;
using Shipwreck.ViewModelUtils.Searching;

namespace Shipwreck.ViewModelUtils.Demo.PresentationFramework;

public sealed class SearchPageWindowViewModel : FrameworkPageViewModel, IFrameworkSearchPageViewModel, ISearchPropertiesHost
{
    private BulkUpdateableCollection<ConditionViewModel> _Conditions;

    private IEnumerable<QueryPropertyInfo> CreateProperties()
    {
        yield return new QueryPropertyInfo { Name = "s", TypeName = nameof(String), DisplayName = "Str" };
        yield return new DateTimeQueryPropertyInfo { Name = "d1", TypeName = nameof(DateTime), DisplayName = "date" };
        yield return new DateTimeQueryPropertyInfo { Name = "d2", TypeName = nameof(DateTimeOffset), DisplayName = "datetime" };
        yield return new BooleanQueryPropertyInfo { Name = "s", TypeName = nameof(Boolean), DisplayName = "B", TrueString = "T", FalseString = "F" };
    }

    Task<QuerySettingsResponse> ISearchPropertiesHost.GetQuerySettingsAsync(CancellationToken cancellationToken)
        => Task.FromResult(QuerySettings);

    private QuerySettingsResponse _QuerySettings;

    public QuerySettingsResponse QuerySettings
    {
        get
        {
            if (_QuerySettings == null)
            {
                var qs = new QuerySettingsResponse();
                qs.Groups.Add(new() { Path = "1", TypeName = "1" });
                qs.Types.Add(new()
                {
                    TypeName = "1",
                    Properties = CreateProperties().ToArray()
                });
                _QuerySettings = qs;
            }
            return _QuerySettings;
        }
    }

    private SearchPropertyGroupViewModel _RootGroup;
    public SearchPropertyGroupViewModel RootGroup => _RootGroup ??= new(this);

    public BulkUpdateableCollection<SearchPropertyViewModel> Properties => RootGroup.Properties;

    public BulkUpdateableCollection<ConditionViewModel> Conditions
        => _Conditions ??= new(RootGroup.Properties.Select(e => e.CreateCondition()));

    public int? PageCount => throw new NotImplementedException();

    public CommandViewModelBase SearchCommand => throw new NotImplementedException();

    public CommandViewModelCollection SearchSubCommands => throw new NotImplementedException();

    public CommandViewModelCollection SelectionCommands => throw new NotImplementedException();

    public bool IsSearching => throw new NotImplementedException();

    public CommandViewModelBase AddConditionsCommand => throw new NotImplementedException();

    public string CurrentQuery => throw new NotImplementedException();

    public ReadOnlyCollection<SortDescription> CurrentOrder => throw new NotImplementedException();

    public int? CurrentPageIndex => throw new NotImplementedException();

    public int? CurrentPageNumber => throw new NotImplementedException();

    public int? CurrentPageSize => throw new NotImplementedException();

    public int? TotalCount => throw new NotImplementedException();

    public int? PageIndex => throw new NotImplementedException();

    public int? PageSize => throw new NotImplementedException();

    public ReadOnlyCollection<SortDescription> Order => throw new NotImplementedException();

    public bool? AllItemsSelected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void BeginSearch()
    {
        throw new NotImplementedException();
    }

    public ConditionViewModel CreateCondition(SearchPropertyViewModel property)
    {
        if (property.IsBoolean)
        {
            return new BooleanConditionViewModel(property);
        }
        if (property.IsDateTime)
        {
            return new DateTimeConditionViewModel(property);
        }
        if (property.IsEnum)
        {
            return new EnumConditionViewModel(property);
        }

        switch (property.TypeName)
        {
            case "Number":
            case nameof(SByte):
            case nameof(Byte):
            case nameof(Int16):
            case nameof(UInt16):
            case nameof(Int32):
            case nameof(UInt32):
            case nameof(Int64):
            case nameof(UInt64):
            case nameof(Single):
            case nameof(Double):
            case nameof(Decimal):
                return new NumberConditionViewModel(property);
        }

        return new StringConditionViewModel(property);
    }

    public ConditionViewModel CreateOrGetCondition(string property)
    {
        throw new NotImplementedException();
    }

    public bool? GetAllItemsSelected(ISelectable item)
    {
        throw new NotImplementedException();
    }

    public ConditionViewModel GetCondition(string property)
        => null;

    public ConditionViewModel GetOrCreateCondition(string property)
        => null;

    public string GetQueryString()
    {
        throw new NotImplementedException();
    }

    public void NavigateTo(int pageIndex)
    {
        throw new NotImplementedException();
    }

    public void OnItemSelectionChanged(ISelectable item, bool newValue)
    {
        throw new NotImplementedException();
    }

    public void SetPageSize(int value)
    {
        throw new NotImplementedException();
    }

    public bool SetParameter(string query, string order, int? pageIndex, int? pageSize)
    {
        throw new NotImplementedException();
    }

    public void ToggleSortKeys(IEnumerable<string> members)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<CommandViewModelBase> CreateConditionCommands(ConditionViewModel condition)
    {
        if (condition is StringConditionViewModel)
        {
            yield break;
        }
        yield return CommandViewModel.Create(
            () => AlertAsync(condition.DisplayName + ": " + condition?.GetType()?.FullName),
            title: "GetType",
            icon: "fas fa-cog",
            style: BorderStyle.OutlineInfo);
    }
}
