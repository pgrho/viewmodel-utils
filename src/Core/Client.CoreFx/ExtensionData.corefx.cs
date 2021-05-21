using System.Text.Json.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    [JsonConverter(typeof(ExtensionDataJsonConverter))]
    public partial class ExtensionData
    {
    }
}
