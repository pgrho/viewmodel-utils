namespace Shipwreck.ViewModelUtils;

public readonly struct FilterOption
{
    public FilterOption(string value, string name, bool isSelected)
    {
        this.Value = value;
        this.Name = name;
        this.IsSelected = isSelected;
    }

    public string Value { get; }
    public string Name { get; }
    public bool IsSelected { get; }
}
