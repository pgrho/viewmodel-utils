namespace Shipwreck.ViewModelUtils.Client;

public class SearchPagePreferenceInfoJsonConverter : MessageObjectJsonConverter<SearchPagePreferenceInfo>
{
    [RequiresUnreferencedCode("Default Constructor")]
    public SearchPagePreferenceInfoJsonConverter() { }

    protected override SearchPagePreferenceInfo CreateInstance() => new SearchPagePreferenceInfo();

    protected override bool TryReadProperty(ref Utf8JsonReader reader, SearchPagePreferenceInfo obj, JsonSerializerOptions options)
        => SearchPagePreferenceInfo.TryReadProperty(ref reader, obj, options);

    protected override void WriteProperties(Utf8JsonWriter writer, SearchPagePreferenceInfo value, JsonSerializerOptions options)
        => SearchPagePreferenceInfo.WriteProperties(writer, value, options);
}
