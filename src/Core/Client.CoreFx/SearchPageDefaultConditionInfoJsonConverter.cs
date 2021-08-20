using System.Text.Json;

namespace Shipwreck.ViewModelUtils.Client
{
    public class SearchPageDefaultConditionInfoJsonConverter : MessageObjectJsonConverter<SearchPageDefaultConditionInfo>
    {
        protected override SearchPageDefaultConditionInfo CreateInstance() => new SearchPageDefaultConditionInfo();

        protected override bool TryReadProperty(ref Utf8JsonReader reader, SearchPageDefaultConditionInfo obj, JsonSerializerOptions options)
            => SearchPageDefaultConditionInfo.TryReadProperty(ref reader, obj, options);

        protected override void WriteProperties(Utf8JsonWriter writer, SearchPageDefaultConditionInfo value, JsonSerializerOptions options)
            => SearchPageDefaultConditionInfo.WriteProperties(writer, value, options);
    }
}
