namespace Shipwreck.ViewModelUtils.Searching;

public interface IFrameworkSearchPageViewModelHandler : IFrameworkPageViewModelHandler
{
    bool TryCreateCondition(SearchPropertyViewModel property, out ConditionViewModel condition)
#if NET9_0_OR_GREATER
    {
        condition = null;
        return false;
    }

#else
    ;
#endif
}
