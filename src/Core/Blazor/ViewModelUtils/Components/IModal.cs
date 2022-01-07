namespace Shipwreck.ViewModelUtils.Components;

public interface IModal : IDisposable
{
    ElementReference ModalElement { get; }
    ModalPresenterBase Presenter { get; set; }
}
