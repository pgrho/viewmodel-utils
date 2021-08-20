using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Shipwreck.ViewModelUtils.Components
{
    public abstract class ModalBase<T> : BindableComponentBase<T>, IModal
          where T : class
    {
        public ElementReference ModalElement { get; set; }

        private bool _IsRendered;

        //[Inject]
        //public SessionState Session { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        #region IsOpen

        private bool _IsOpen;

        [Parameter]
        public bool IsOpen
        {
            get => _IsOpen;
            set
            {
                if (value != _IsOpen)
                {
                    _IsOpen = value;
                    OnIsOpenChanged();
                }
            }
        }

        protected virtual void OnIsOpenChanged()
        {
            if (_IsOpen || _IsRendered)
            {
                StateHasChanged();
            }
        }

        #endregion IsOpen

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var t = base.OnAfterRenderAsync(firstRender);
            if (t != null)
            {
                await t.ConfigureAwait(false);
            }

            if (ModalElement.Id == null)
            {
                throw new Exception();
            }

            if (_IsOpen)
            {
                await JS.InvokeVoidAsync("Shipwreck.ViewModelUtils.toggleModal", ModalElement, true, DotNetObjectReference.Create(this)).ConfigureAwait(false);
            }
            else if (_IsRendered)
            {
                await JS.InvokeVoidAsync("Shipwreck.ViewModelUtils.toggleModal", ModalElement, false, DotNetObjectReference.Create(this)).ConfigureAwait(false);
            }

            _IsRendered = true;
        }

        [JSInvokable]
        public virtual void OnClosed()
        {
            _IsOpen = false;
        }

        public void Dispose()
            => Dispose(false);

        protected virtual void Dispose(bool disposing) => IsOpen = false;
    }
}
