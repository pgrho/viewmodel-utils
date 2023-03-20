namespace Shipwreck.ViewModelUtils.Components;

public interface IBindableComponent : IHasBindableComponent
{
    Task InvokeAsync(Action workItem);

    Task InvokeAsync(Func<Task> workItem);

    object DataContext { get; }

    IDisposable PushPropertyChangedExpectation(string expectedPropertyName = null);
}
