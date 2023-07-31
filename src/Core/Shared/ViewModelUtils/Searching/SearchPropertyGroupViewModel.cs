namespace Shipwreck.ViewModelUtils.Searching;

public sealed class SearchPropertyGroupViewModel : ObservableModel
{
    internal SearchPropertyGroupViewModel(SearchPropertiesModalViewModel modal, string ancestorPath, IEnumerable<SearchPropertyViewModel> properties)
    {
        Modal = modal;
        AncestorPath = ancestorPath;
        _IsExpanded = string.IsNullOrEmpty(ancestorPath);
        Items = Array.AsReadOnly(properties.ToArray());
    }

    SearchPropertiesModalViewModel Modal { get; }
    public string DisplayName
        => AncestorPath == null ? null
        : (Modal.SearchPage.Properties.FirstOrDefault(e => e.Name == AncestorPath)?.DisplayName ?? AncestorPath);

    public string AncestorPath { get; }

    #region IsExpanded

    private bool _IsExpanded;

    public bool IsExpanded
    {
        get => _IsExpanded;
        set => SetProperty(ref _IsExpanded, value);
    }

    public void ToggleIsExpanded()
        => IsExpanded = !IsExpanded;

    #endregion IsExpanded

    public ReadOnlyCollection<SearchPropertyViewModel> Items { get; }
}
