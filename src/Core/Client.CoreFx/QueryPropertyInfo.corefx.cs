using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    [JsonConverter(typeof(QueryPropertyInfoJsonConverter))]
    public partial class QueryPropertyInfo
    {
        protected internal static bool TryReadProperty(ref Utf8JsonReader reader, QueryPropertyInfo obj, JsonSerializerOptions options)
        {
            if (reader.ValueTextEquals(nameof(obj.Name)))
            {
                obj.Name = reader.ReadString();
                return true;
            }
            if (reader.ValueTextEquals(nameof(obj.DisplayName)))
            {
                obj.DisplayName = reader.ReadString();
                return true;
            }
            if (reader.ValueTextEquals(nameof(obj.TypeName)))
            {
                obj.TypeName = reader.ReadString();
                return true;
            }
            if (reader.ValueTextEquals(nameof(obj.DefaultOperator)))
            {
                obj.DefaultOperator = reader.ReadString();
                return true;
            }
            return false;
        }

        protected internal static void WriteProperties(Utf8JsonWriter writer, QueryPropertyInfo value, JsonSerializerOptions options)
        {
            writer.WriteString(nameof(value.Name), value.Name);
            writer.WriteString(nameof(value.DisplayName), value.DisplayName);
            writer.WriteString(nameof(value.TypeName), value.TypeName);
            writer.WriteString(nameof(value.DefaultOperator), value.DefaultOperator);
        }
    }
}
