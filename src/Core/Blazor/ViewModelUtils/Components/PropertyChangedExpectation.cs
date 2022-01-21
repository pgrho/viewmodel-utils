namespace Shipwreck.ViewModelUtils.Components;

internal class PropertyChangedExpectation : IDisposable
{
    private readonly Stack<PropertyChangedExpectation> _Stack;

    public PropertyChangedExpectation(Stack<PropertyChangedExpectation> stack, string propertyName)
    {
        _Stack = stack;
        PropertyName = propertyName;
    }

    public string PropertyName { get; }
    private bool _IsIgnored;

    internal bool ShouldIgnorePropertyChanged(string propertyName)
    {
        if (!_IsIgnored && (PropertyName == null || propertyName == PropertyName))
        {
            _IsIgnored = true;
            return true;
        }
        return false;
    }

    public void Dispose()
    {
        if (_Stack.TryPeek(out var r)
            && r == this)
        {
            _Stack.Pop();
        }
    }
}
