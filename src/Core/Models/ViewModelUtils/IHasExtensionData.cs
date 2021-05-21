using System.Collections.Generic;

namespace Shipwreck.ViewModelUtils
{
    public interface IHasExtensionData
    {
        bool IsExtensionDataSet { get; }

        IEnumerable<KeyValuePair<string, string>> GetExtensionData();
    }
}
