using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    [JsonConverter(typeof(QueryPropertyInfoJsonConverter))]
    public partial class DateTimeQueryPropertyInfo
    {
        protected internal static bool TryReadProperty(ref Utf8JsonReader reader, DateTimeQueryPropertyInfo obj, JsonSerializerOptions options)
        {
            if (reader.ValueTextEquals(nameof(obj.IsDate)))
            {
                obj.IsDate = reader.ReadBoolean();
                return true;
            }
            return QueryPropertyInfo.TryReadProperty(ref reader, obj, options);
        }

        protected internal static void WriteProperties(Utf8JsonWriter writer, DateTimeQueryPropertyInfo value, JsonSerializerOptions options)
        {
            writer.WriteBoolean(nameof(value.IsDate), value.IsDate);
            QueryPropertyInfo.WriteProperties(writer, value, options);
        }
    }
}
