namespace Shipwreck.ViewModelUtils;

partial interface IFilterable
{
    bool TryShowPopover(string key, ComponentBase component, ElementReference targetElement)
        => false;
}
