namespace Shipwreck.ViewModelUtils.Searching;

public class SearchPropertyGroupViewModel : ObservableModel
{
    protected internal SearchPropertyGroupViewModel(SearchPropertiesModalViewModel modal, string ancestorPath, IEnumerable<SearchPropertyViewModel> properties, string displayName = null)
    {
        Modal = modal;
        AncestorPath = ancestorPath;
        _IsExpanded = string.IsNullOrEmpty(ancestorPath);
        Items = Array.AsReadOnly(properties.ToArray());
        _DisplayName = displayName;
    }

    SearchPropertiesModalViewModel Modal { get; }

    #region DisplayName

    private string _DisplayName;

    public string DisplayName
        => (_DisplayName ??= GetDisplayName()).TrimOrNull();

    protected virtual string GetDisplayName()
        => AncestorPath == null ? string.Empty
        : (Modal.Properties.FirstOrDefault(e => e.Name == AncestorPath)?.DisplayName ?? AncestorPath);

    #endregion DisplayName

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
