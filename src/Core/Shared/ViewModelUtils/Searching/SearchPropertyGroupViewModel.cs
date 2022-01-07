namespace Shipwreck.ViewModelUtils.Searching;

public sealed class SearchPropertyGroupViewModel : ObservableModel
{
    internal SearchPropertyGroupViewModel(SearchPropertiesModalViewModel modal, string ancestorPath, IEnumerable<SearchPropertyViewModel> properties)
    {
        Modal = modal;
        AncestorPath = ancestorPath;
        Items = Array.AsReadOnly(properties.ToArray());
    }

    SearchPropertiesModalViewModel Modal { get; }
    public string DisplayName
        => AncestorPath == null ? null
        : (Modal.SearchPage.Properties.FirstOrDefault(e => e.Name == AncestorPath)?.DisplayName ?? AncestorPath);

    public string AncestorPath { get; }

    public ReadOnlyCollection<SearchPropertyViewModel> Items { get; }
}
