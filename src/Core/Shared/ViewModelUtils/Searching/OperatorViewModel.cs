namespace Shipwreck.ViewModelUtils.Searching;

public sealed class OperatorViewModel
{
    public OperatorViewModel(string token, string displayName)
    {
        Token = token;
        DisplayName = displayName;
    }

    public string Token { get; }
    public string DisplayName { get; }
}
