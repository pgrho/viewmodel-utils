using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    public sealed class EnumFieldInfoJsonConverter : JsonConverter<EnumFieldInfo>
    {
        public override bool CanConvert(Type typeToConvert)
          => typeof(EnumFieldInfo).IsAssignableFrom(typeToConvert);

        public override EnumFieldInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new InvalidOperationException();
            }

            var ret = new EnumFieldInfo();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return ret;
                }
                else if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (!EnumFieldInfo.TryReadProperty(ref reader, ret, options))
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

        internal List<EnumFieldInfo> ReadList(ref Utf8JsonReader reader, JsonSerializerOptions options)
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
                List<EnumFieldInfo> list = null;
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
                    (list ??= new List<EnumFieldInfo>()).Add(Read(ref reader, typeof(EnumFieldInfo), options));
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public override void Write(Utf8JsonWriter writer, EnumFieldInfo value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();

            EnumFieldInfo.WriteProperties(writer, value, options);

            writer.WriteEndObject();
        }
    }
}
