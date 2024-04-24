namespace Shipwreck.ViewModelUtils.Searching;

public interface ISearchPropertiesHost : IHasPageLogger
{
    QuerySettingsResponse QuerySettings { get; }

    SearchPropertyGroupViewModel RootGroup { get; }
    BulkUpdateableCollection<ConditionViewModel> Conditions { get; }
    CommandViewModelBase AddConditionsCommand { get; }

    string GetDisplayName(QueryGroupInfo group)
#if NET7_0_OR_GREATER
        => null;

#else
    ;
#endif

    string GetDisplayName(QueryPropertyInfo property)
#if NET7_0_OR_GREATER
        => null;

#else
    ;
#endif

    bool ShouldInclude(QueryGroupInfo group)
#if NET7_0_OR_GREATER
        => true;

#else
    ;
#endif

    bool ShouldInclude(QueryPropertyInfo property)
#if NET7_0_OR_GREATER
        => true;

#else
    ;
#endif

    bool TryCreateCondition(SearchPropertyViewModel property, out ConditionViewModel condition)
#if NET7_0_OR_GREATER
    {
        condition = null;
        return false;
    }

#else
    ;
#endif

    IEnumerable<CommandViewModelBase> CreateConditionCommands(ConditionViewModel condition)
#if NET7_0_OR_GREATER
        => null;

#else
    ;
#endif

    bool IsSearching { get; }

    void BeginSearch();
}
