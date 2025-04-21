namespace Shipwreck.ViewModelUtils.Client;

[JsonConverter(typeof(SearchPagePreferenceInfoJsonConverter))]
public partial class SearchPagePreferenceInfo
{
    protected internal static bool TryReadProperty(ref Utf8JsonReader reader, SearchPagePreferenceInfo obj, JsonSerializerOptions options)
    {
        if (reader.ValueTextEquals(nameof(obj.Conditions)))
        {
            obj.Conditions = new SearchPageDefaultConditionInfoJsonConverter().ReadList(ref reader, options);
            return true;
        }
        if (reader.ValueTextEquals(nameof(obj.Orders)))
        {
            obj.Orders = reader.ReadStringList();
            return true;
        }
        return false;
    }

    protected internal static void WriteProperties(Utf8JsonWriter writer, SearchPagePreferenceInfo value, JsonSerializerOptions options)
    {
        if (value.ShouldSerializeConditions())
        {
            writer.WritePropertyName(nameof(value.Conditions));
            writer.WriteStartArray();
            foreach (var e in value.Conditions)
            {
                new SearchPageDefaultConditionInfoJsonConverter().Write(writer, e, options);
            }
            writer.WriteEndArray();
        }
        if (value.ShouldSerializeOrders())
        {
            writer.WritePropertyName(nameof(value.Orders));
            writer.WriteStartArray();
            foreach (var e in value.Orders)
            {
                if (e == null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    writer.WriteStringValue(e);
                }
            }
            writer.WriteEndArray();
        }
    }
}
