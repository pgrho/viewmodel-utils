using Newtonsoft.Json;

namespace Shipwreck.ViewModelUtils.Client
{
    [JsonConverter(typeof(ExtensionDataJsonConverter))]
    public partial class ExtensionData
    {
    }
}
