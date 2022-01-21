namespace Shipwreck.ViewModelUtils.Components;

public interface IBindableComponentsHost
{
    IDisposable PushPropertyChangedExpectation(string expectedPropertyName = null);
}
