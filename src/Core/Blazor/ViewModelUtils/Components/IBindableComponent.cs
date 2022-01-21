namespace Shipwreck.ViewModelUtils.Components;

public interface IBindableComponent
{
    object DataContext { get; }
    IDisposable PushPropertyChangedExpectation(string expectedPropertyName = null);
}
