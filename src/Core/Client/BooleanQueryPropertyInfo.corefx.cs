namespace Shipwreck.ViewModelUtils.Client;

[JsonConverter(typeof(QueryPropertyInfoJsonConverter))]
public partial class BooleanQueryPropertyInfo
{
    protected internal static bool TryReadProperty(ref Utf8JsonReader reader, BooleanQueryPropertyInfo obj, JsonSerializerOptions options)
    {
        if (reader.ValueTextEquals(nameof(obj.TrueString)))
        {
            obj.TrueString = reader.ReadString();
            return true;
        }
        if (reader.ValueTextEquals(nameof(obj.FalseString)))
        {
            obj.FalseString = reader.ReadString();
            return true;
        }

        return QueryPropertyInfo.TryReadProperty(ref reader, obj, options);
    }

    protected internal static void WriteProperties(Utf8JsonWriter writer, BooleanQueryPropertyInfo value, JsonSerializerOptions options)
    {
        writer.WriteString(nameof(value.TrueString), value.TrueString);
        writer.WriteString(nameof(value.FalseString), value.FalseString);
        QueryPropertyInfo.WriteProperties(writer, value, options);
    }
}
