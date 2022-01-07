namespace Shipwreck.ViewModelUtils;

public interface ISelectable
{
    bool IsSelected { get; set; }
    void SetIsSelected(bool value, bool notifyHost);
}
