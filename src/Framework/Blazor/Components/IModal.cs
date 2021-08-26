using System;
using Microsoft.AspNetCore.Components;

namespace Shipwreck.ViewModelUtils.Components
{
    public interface IModal : IDisposable
    {
        ElementReference ModalElement { get; }
        ModalPresenter Presenter { get; set; }
    }
}
