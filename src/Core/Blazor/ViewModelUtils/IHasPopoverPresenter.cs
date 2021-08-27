using Shipwreck.ViewModelUtils.Components;

namespace Shipwreck.ViewModelUtils
{
    public interface IHasPopoverPresenter
    {
        ModalPresenterBase PopoverPresenter { get; }
    }
}
