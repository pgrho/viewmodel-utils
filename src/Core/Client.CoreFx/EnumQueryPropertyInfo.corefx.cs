using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    [JsonConverter(typeof(QueryPropertyInfoJsonConverter))]
    public partial class EnumQueryPropertyInfo
    {
        protected internal static bool TryReadProperty(ref Utf8JsonReader reader, EnumQueryPropertyInfo obj, JsonSerializerOptions options)
        {
            if (reader.ValueTextEquals(nameof(obj.IsFlags)))
            {
                obj.IsFlags = reader.ReadBoolean();
                return true;
            }
            if (reader.ValueTextEquals(nameof(obj.Fields)))
            {
                obj.Fields = new EnumFieldInfoJsonConverter().ReadList(ref reader, options);
                return true;
            }

            return QueryPropertyInfo.TryReadProperty(ref reader, obj, options);
        }

        protected internal static void WriteProperties(Utf8JsonWriter writer, EnumQueryPropertyInfo value, JsonSerializerOptions options)
        {
            writer.WriteBoolean(nameof(value.IsFlags), value.IsFlags);

            if (value.Fields != null)
            {
                writer.WritePropertyName(nameof(value.Fields));
                writer.WriteStartArray();
                foreach (var e in value.Fields)
                {
                    new EnumFieldInfoJsonConverter().Write(writer, e, options);
                }
                writer.WriteEndArray();
            }
            QueryPropertyInfo.WriteProperties(writer, value, options);
        }
    }
}
