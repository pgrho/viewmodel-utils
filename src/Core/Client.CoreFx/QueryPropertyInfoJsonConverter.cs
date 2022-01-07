namespace Shipwreck.ViewModelUtils.Client;

public class QueryPropertyInfoJsonConverter : JsonConverter<QueryPropertyInfo>
{
    private delegate bool TryReadPropertyHandler(ref Utf8JsonReader reader, JsonSerializerOptions options);

    public override bool CanConvert(Type typeToConvert)
        => typeof(QueryPropertyInfo).IsAssignableFrom(typeToConvert);

    public override QueryPropertyInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var jo = JsonDocument.ParseValue(ref reader).RootElement;

        QueryPropertyInfo ret = null;
        TryReadPropertyHandler tryRead = null;

        if (jo.TryGetProperty(nameof(QueryPropertyInfo.TypeName), out var tne))
        {
            switch (tne.GetString())
            {
                case nameof(DateTime):
                case nameof(DateTimeOffset):
                    var dr = new DateTimeQueryPropertyInfo();
                    ret = dr;
                    tryRead = (ref Utf8JsonReader r, JsonSerializerOptions op) => DateTimeQueryPropertyInfo.TryReadProperty(ref r, dr, options);
                    break;

                case nameof(Boolean):
                    var br = new BooleanQueryPropertyInfo();
                    ret = br;
                    tryRead = (ref Utf8JsonReader r, JsonSerializerOptions op) => BooleanQueryPropertyInfo.TryReadProperty(ref r, br, options);
                    break;

                case nameof(Enum):
                    var er = new EnumQueryPropertyInfo();
                    ret = er;
                    tryRead = (ref Utf8JsonReader r, JsonSerializerOptions op) => EnumQueryPropertyInfo.TryReadProperty(ref r, er, options);
                    break;
            }
        }

        if (jo.TryGetProperty(nameof(EnumQueryPropertyInfo.Fields), out var fe))
        {
            var er = new EnumQueryPropertyInfo();
            ret = er;
            tryRead = (ref Utf8JsonReader r, JsonSerializerOptions op) => EnumQueryPropertyInfo.TryReadProperty(ref r, er, options);
        }

        if (ret == null)
        {
            ret = new QueryPropertyInfo();
            tryRead = (ref Utf8JsonReader r, JsonSerializerOptions op) => QueryPropertyInfo.TryReadProperty(ref r, ret, options);
        }

        var nr = new Utf8JsonReader(Encoding.UTF8.GetBytes(jo.GetRawText()).AsSpan());

        if (nr.Read())
        {
            while (nr.Read())
            {
                if (nr.TokenType == JsonTokenType.EndObject)
                {
                    return ret;
                }
                else if (nr.TokenType == JsonTokenType.PropertyName)
                {
                    if (!tryRead(ref nr, options))
                    {
                        nr.TrySkip();
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected TokenType={nr.TokenType}");
                }
            }
        }

        throw new InvalidOperationException("Unexpected EOL");
    }

    public override void Write(Utf8JsonWriter writer, QueryPropertyInfo value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        if (value is BooleanQueryPropertyInfo bp)
        {
            writer.WriteStartObject();
            BooleanQueryPropertyInfo.WriteProperties(writer, bp, options);
            writer.WriteEndObject();
        }
        else if (value is DateTimeQueryPropertyInfo dp)
        {
            writer.WriteStartObject();
            DateTimeQueryPropertyInfo.WriteProperties(writer, dp, options);
            writer.WriteEndObject();
        }
        else if (value is EnumQueryPropertyInfo ep)
        {
            writer.WriteStartObject();
            EnumQueryPropertyInfo.WriteProperties(writer, ep, options);
            writer.WriteEndObject();
        }
        else
        {
            writer.WriteStartObject();

            QueryPropertyInfo.WriteProperties(writer, value, options);

            writer.WriteEndObject();
        }
    }
}
