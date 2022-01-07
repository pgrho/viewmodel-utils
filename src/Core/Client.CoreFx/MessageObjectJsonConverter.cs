namespace Shipwreck.ViewModelUtils.Client;

public abstract partial class MessageObjectJsonConverter<T> : JsonConverter<T>
    where T : class
{
    public override bool CanConvert(Type typeToConvert)
        => typeof(T).IsAssignableFrom(typeToConvert);

    protected abstract T CreateInstance();

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new InvalidOperationException();
        }

        var ret = CreateInstance();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return ret;
            }
            else if (reader.TokenType == JsonTokenType.PropertyName)
            {
                if (!TryReadProperty(ref reader, ret, options))
                {
                    reader.TrySkip();
                }
            }
            else
            {
                throw new InvalidOperationException($"Unexpected TokenType={reader.TokenType} TokenStartIndex={reader.TokenStartIndex}");
            }
        }

        throw new InvalidOperationException("Unexpected EOL");
    }

    public List<T> ReadList(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (!reader.Read())
        {
            throw new InvalidOperationException();
        }
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            List<T> list = null;
            for (; ; )
            {
                if (!reader.Read())
                {
                    throw new InvalidOperationException();
                }
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return list;
                }
                (list ??= new List<T>()).Add(Read(ref reader, typeof(T), options));
            }
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        WriteProperties(writer, value, options);

        writer.WriteEndObject();
    }

    protected abstract bool TryReadProperty(ref Utf8JsonReader reader, T obj, JsonSerializerOptions options);

    protected abstract void WriteProperties(Utf8JsonWriter writer, T value, JsonSerializerOptions options);
}
