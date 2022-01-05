using System;
using Microsoft.JSInterop;
using Shipwreck.ViewModelUtils.Components;

namespace Shipwreck.ViewModelUtils
{
    public partial class FrameworkPageViewModel : IHasJSRuntime
    {
        public struct ProcessingDisabled : IDisposable
        {
            public void Dispose()
            {
            }
        }

        protected FrameworkPageViewModel(FrameworkPageBase page)
        {
            Page = page;
        }

        public FrameworkPageBase Page { get; }

        public IJSRuntime JS => Page.JS;

        public ProcessingDisabled DisableProcessing()
            => default;
    }
}
