namespace Shipwreck.ViewModelUtils.Client
{
    public interface IMessageObjectCollection
    {
        FrameworkMessageObject Owner { get; }
        FrameworkMessageBase Message { get; }
    }
}
