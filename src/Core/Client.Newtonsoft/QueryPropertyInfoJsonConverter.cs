using System;
using Newtonsoft.Json;

namespace Shipwreck.ViewModelUtils.Client
{
    public class QueryPropertyInfoJsonConverter : JsonConverter<QueryPropertyInfo>
    {
        public override QueryPropertyInfo ReadJson(JsonReader reader, Type objectType, QueryPropertyInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else if (reader.TokenType != JsonToken.StartObject)
            {
                throw new InvalidOperationException();
            }

            var r = new QueryPropertyInfo();

            BooleanQueryPropertyInfo getOrCreateBoolean()
                => (r as BooleanQueryPropertyInfo) ?? (BooleanQueryPropertyInfo)(r = new BooleanQueryPropertyInfo
                {
                    Name = r.Name,
                    DisplayName = r.DisplayName,
                    TypeName = r.TypeName,
                    DefaultOperator = r.DefaultOperator,
                });

            DateTimeQueryPropertyInfo getOrCreateDateTime()
                => (r as DateTimeQueryPropertyInfo) ?? (DateTimeQueryPropertyInfo)(r = new DateTimeQueryPropertyInfo
                {
                    Name = r.Name,
                    DisplayName = r.DisplayName,
                    TypeName = r.TypeName,
                    DefaultOperator = r.DefaultOperator,
                });

            EnumQueryPropertyInfo getOrCreateEnum()
                => (r as EnumQueryPropertyInfo) ?? (EnumQueryPropertyInfo)(r = new EnumQueryPropertyInfo
                {
                    Name = r.Name,
                    DisplayName = r.DisplayName,
                    TypeName = r.TypeName,
                    DefaultOperator = r.DefaultOperator,
                });

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.EndObject:
                        return r;

                    case JsonToken.PropertyName:
                        var propertyName = reader.Value?.ToString();

                        if (!reader.Read())
                        {
                            throw new InvalidOperationException();
                        }

                        switch (propertyName)
                        {
                            case nameof(r.TypeName):
                                var tn = reader.Value?.ToString();

                                switch (tn)
                                {
                                    case nameof(DateTime):
                                    case nameof(DateTimeOffset):
                                        getOrCreateDateTime();
                                        break;

                                    case nameof(Boolean):
                                        getOrCreateBoolean();
                                        break;

                                    case nameof(Enum):
                                        getOrCreateEnum();
                                        break;
                                }

                                r.TypeName = tn;
                                break;

                            case nameof(r.Name):
                                r.Name = reader.Value?.ToString();
                                break;

                            case nameof(r.DisplayName):
                                r.DisplayName = reader.Value?.ToString();
                                break;

                            case nameof(r.DefaultOperator):
                                r.DefaultOperator = reader.Value?.ToString();
                                break;

                            case nameof(BooleanQueryPropertyInfo.TrueString):
                                getOrCreateBoolean().TrueString = reader.Value?.ToString();
                                break;

                            case nameof(BooleanQueryPropertyInfo.FalseString):
                                getOrCreateBoolean().FalseString = reader.Value?.ToString();
                                break;

                            case nameof(DateTimeQueryPropertyInfo.IsDate):
                                getOrCreateDateTime().IsDate = reader.Value is bool bds
                                    || reader.Value is string ds && bool.TryParse(ds, out bds)
                                    ? bds : false;
                                break;

                            case nameof(EnumQueryPropertyInfo.Fields):
                                var er = getOrCreateEnum();
                                if (reader.TokenType == JsonToken.Null)
                                {
                                    er.Fields = null;
                                }
                                else if (reader.TokenType != JsonToken.StartArray)
                                {
                                    throw new InvalidOperationException();
                                }
                                else
                                {
                                    EnumFieldInfo f = null;
                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                    {
                                        switch (reader.TokenType)
                                        {
                                            case JsonToken.EndObject:
                                                if (f != null)
                                                {
                                                    er.Fields.Add(f);
                                                    f = null;
                                                }
                                                break;

                                            case JsonToken.StartObject:
                                                f = new EnumFieldInfo();
                                                break;

                                            case JsonToken.PropertyName:

                                                switch (reader.Value?.ToString())
                                                {
                                                    case nameof(f.Name):
                                                        f.Name = reader.ReadAsString();
                                                        break;

                                                    case nameof(f.DisplayName):
                                                        f.DisplayName = reader.ReadAsString();
                                                        break;

                                                    case nameof(f.Value):
                                                        f.Value = (long)reader.ReadAsDouble();
                                                        break;

                                                    default:
                                                        if (!reader.Read())
                                                        {
                                                            throw new InvalidOperationException();
                                                        }
                                                        break;
                                                }

                                                break;
                                        }
                                    }
                                }

                                break;
                        }

                        break;
                }
            }

            throw new InvalidOperationException();
        }

        public override void WriteJson(JsonWriter writer, QueryPropertyInfo value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            if (value is BooleanQueryPropertyInfo b)
            {
                writer.WritePropertyName(nameof(b.TrueString));
                writer.WriteValue(b.TrueString);

                writer.WritePropertyName(nameof(b.FalseString));
                writer.WriteValue(b.FalseString);
            }
            else if (value is DateTimeQueryPropertyInfo d)
            {
                writer.WritePropertyName(nameof(d.IsDate));
                writer.WriteValue(d.IsDate);
            }
            else if (value is EnumQueryPropertyInfo e)
            {
                writer.WritePropertyName(nameof(e.Fields));
                writer.WriteStartArray();
                foreach (var f in e.Fields)
                {
                    writer.WriteStartObject();

                    writer.WritePropertyName(nameof(f.Value));
                    writer.WriteValue(f.Value);

                    writer.WritePropertyName(nameof(f.Name));
                    writer.WriteValue(f.Name);

                    writer.WritePropertyName(nameof(f.DisplayName));
                    writer.WriteValue(f.DisplayName);

                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }

            writer.WritePropertyName(nameof(value.Name));
            writer.WriteValue(value.Name);

            writer.WritePropertyName(nameof(value.DisplayName));
            writer.WriteValue(value.DisplayName);

            writer.WritePropertyName(nameof(value.TypeName));
            writer.WriteValue(value.TypeName);

            writer.WritePropertyName(nameof(value.DefaultOperator));
            writer.WriteValue(value.DefaultOperator);

            writer.WriteEndObject();
        }
    }
}
