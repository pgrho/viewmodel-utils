namespace Shipwreck.ViewModelUtils;

public interface ISelectablesHost
{
    bool? AllItemsSelected { get; set; }

    bool? GetAllItemsSelected(ISelectable item);

    void OnItemSelectionChanged(ISelectable item, bool newValue);
}
