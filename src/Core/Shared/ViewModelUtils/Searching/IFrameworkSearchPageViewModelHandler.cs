namespace Shipwreck.ViewModelUtils.Searching;

public interface IFrameworkSearchPageViewModelHandler : IFrameworkPageViewModelHandler
{
    bool TryCreateCondition(SearchPropertyViewModel property, out ConditionViewModel condition)
#if NET7_0_OR_GREATER
    {
        condition = null;
        return false;
    }

#else
    ;
#endif
}
