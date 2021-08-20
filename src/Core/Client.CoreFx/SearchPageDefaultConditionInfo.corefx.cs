using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    [JsonConverter(typeof(SearchPageDefaultConditionInfoJsonConverter))]
    public partial class SearchPageDefaultConditionInfo
    {
        protected internal static bool TryReadProperty(ref Utf8JsonReader reader, SearchPageDefaultConditionInfo obj, JsonSerializerOptions options)
        {
            if (reader.ValueTextEquals(nameof(obj.Name)))
            {
                obj.Name = reader.ReadString();
                return true;
            }
            if (reader.ValueTextEquals(nameof(obj.Operator)))
            {
                obj.Operator = reader.ReadString();
                return true;
            }
            if (reader.ValueTextEquals(nameof(obj.DefaultValue)))
            {
                obj.DefaultValue = reader.ReadString();
                return true;
            }
            return false;
        }

        protected internal static void WriteProperties(Utf8JsonWriter writer, SearchPageDefaultConditionInfo value, JsonSerializerOptions options)
        {
            writer.WriteString(nameof(value.Name), value.Name);
            writer.WriteString(nameof(value.Operator), value.Operator);
            writer.WriteString(nameof(value.DefaultValue), value.DefaultValue);
        }
    }
}
