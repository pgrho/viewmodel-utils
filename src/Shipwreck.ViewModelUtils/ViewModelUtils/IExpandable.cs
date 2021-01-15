namespace Shipwreck.ViewModelUtils
{
    public interface IExpandable
    {
        bool IsExpandable { get; }
        bool IsExpanded { get; set; }
    }
}
