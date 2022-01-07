namespace Shipwreck.ViewModelUtils;

internal interface IEnumDataAnnotations
{
    // TODO: move to Shipwreck.ReflectionUtils
    string GetDisplayName(object value);

    string GetShortName(object value);

    string GetGroupName(object value);

    string GetDescription(object value);

    string GetPrompt(object value);
}
