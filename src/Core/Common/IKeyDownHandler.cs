namespace Shipwreck.ViewModelUtils;

public interface IKeyDownHandler
{
    bool GetIsFocused();

    bool HandleKeyDown(string keys, bool replaceText);
}
