namespace Shipwreck.ViewModelUtils.JSInterop
{
    public interface IScrollEventListener : IWindowResizeEventListener
    {
        void OnElementScroll(string jsonScrollInfo);
    }
}
