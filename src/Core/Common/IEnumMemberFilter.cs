namespace Shipwreck.ViewModelUtils;

public interface IEnumMemberFilter : IMemberFilter
{
    IEnumerable<FilterOption> EnumerateOptions();
}
