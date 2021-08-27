using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Shipwreck.ViewModelUtils.Components
{
    public abstract class BootstrapModalBase<T> : ModalBase<T>
          where T : class
    {

        protected override ValueTask ShowAsyncCore()
            => JS.InvokeVoidAsync("Shipwreck.ViewModelUtils.toggleModal", ModalElement, true, DotNetObjectReference.Create(this));

        protected override ValueTask HideAsyncCore()
            => JS.InvokeVoidAsync("Shipwreck.ViewModelUtils.toggleModal", ModalElement, false, DotNetObjectReference.Create(this));
    }
}
