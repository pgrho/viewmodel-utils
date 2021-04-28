using Newtonsoft.Json;

namespace Shipwreck.ViewModelUtils.Client
{
    [JsonConverter(typeof(QueryPropertyInfoJsonConverter))]
    public partial class QueryPropertyInfo
    {
    }
}
