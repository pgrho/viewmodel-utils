namespace Shipwreck.ViewModelUtils;

public interface IEnumMemberFilter : IMemberFilter
{
    IEnumerable<(string value, string name, bool isSelected)> EnumerateOptions();
}
