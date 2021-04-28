namespace Shipwreck.ViewModelUtils.Searching
{
    public sealed class OptionViewModel<T>
    {
        public OptionViewModel(T value, string stringValue, string displayName)
        {
            Value = value;
            StringValue = stringValue;
            DisplayName = displayName;
        }

        public T Value { get; }
        public string StringValue { get; }
        public string DisplayName { get; }
    }
}
