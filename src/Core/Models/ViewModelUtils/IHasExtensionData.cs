namespace Shipwreck.ViewModelUtils;

public interface IHasExtensionData
{
    bool IsExtensionDataSet { get; }

    IEnumerable<KeyValuePair<string, string>> GetExtensionData();
}
