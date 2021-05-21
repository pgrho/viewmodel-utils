using System;
using Newtonsoft.Json;

namespace Shipwreck.ViewModelUtils.Client
{
    public sealed class ExtensionDataJsonConverter : JsonConverter<ExtensionData>
    {
        public override ExtensionData ReadJson(JsonReader reader, Type objectType, ExtensionData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                var ret = new ExtensionData();
                string pn = null;
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.PropertyName:
                            pn = string.Intern(reader.Value?.ToString());
                            break;

                        case JsonToken.String:
                            if (!string.IsNullOrEmpty(pn))
                            {
                                var v = string.Intern(reader.Value?.ToString());
                                if (!string.IsNullOrEmpty(v))
                                {
                                    ret[pn] = v;
                                }
                            }
                            pn = null;
                            break;

                        case JsonToken.Null:
                            pn = null;
                            break;

                        case JsonToken.EndObject:
                            return ret;

                        default:
                            break;
                    }
                }
            }
            throw new InvalidCastException();
        }

        public override void WriteJson(JsonWriter writer, ExtensionData value, JsonSerializer serializer)
        {
            if (value?.Count > 0)
            {
                writer.WriteStartObject();
                foreach (var kv in value)
                {
                    if (!string.IsNullOrEmpty(kv.Key) && !string.IsNullOrEmpty(kv.Value))
                    {
                        writer.WritePropertyName(kv.Key);
                        writer.WriteValue(kv.Value);
                    }
                }
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}
