using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    [JsonConverter(typeof(EnumFieldInfoJsonConverter))]
    public partial class EnumFieldInfo
    {
        internal static bool TryReadProperty(ref Utf8JsonReader reader, EnumFieldInfo obj, JsonSerializerOptions options)
        {
            if (reader.ValueTextEquals(nameof(obj.Value)))
            {
                obj.Value = reader.ReadInt64();
                return true;
            }
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
            return false;
        }

        internal static void WriteProperties(Utf8JsonWriter writer, EnumFieldInfo value, JsonSerializerOptions options)
        {
            writer.WriteNumber(nameof(value.Value), value.Value);
            writer.WriteString(nameof(value.Name), value.Name);
            writer.WriteString(nameof(value.DisplayName), value.DisplayName);
        }
    }
}
