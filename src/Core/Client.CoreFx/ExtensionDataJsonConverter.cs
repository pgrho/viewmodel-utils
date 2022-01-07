namespace Shipwreck.ViewModelUtils.Client;

public sealed class ExtensionDataJsonConverter : JsonConverter<ExtensionData>
{
    public override bool CanConvert(Type typeToConvert)
        => typeof(ExtensionData).IsAssignableFrom(typeToConvert);

    public override ExtensionData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            var ret = new ExtensionData();
            string pn = null;
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        pn = string.Intern(reader.GetString());
                        break;

                    case JsonTokenType.String:
                        if (!string.IsNullOrEmpty(pn))
                        {
                            var v = string.Intern(reader.GetString());
                            if (!string.IsNullOrEmpty(v))
                            {
                                ret[pn] = v;
                            }
                        }
                        pn = null;
                        break;

                    case JsonTokenType.Null:
                        pn = null;
                        break;

                    case JsonTokenType.EndObject:
                        return ret;

                    default:
                        reader.TrySkip();
                        break;
                }
            }
        }
        throw new InvalidCastException();
    }

    public override void Write(Utf8JsonWriter writer, ExtensionData value, JsonSerializerOptions options)
    {
        if (value?.Count > 0)
        {
            writer.WriteStartObject();
            foreach (var kv in value)
            {
                if (!string.IsNullOrEmpty(kv.Key) && !string.IsNullOrEmpty(kv.Value))
                {
                    writer.WriteString(kv.Key, kv.Value);
                }
            }
            writer.WriteEndObject();
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
