using Microsoft.JSInterop;
using Shipwreck.ViewModelUtils.Components;

namespace Shipwreck.ViewModelUtils
{
    public partial class FrameworkPageViewModel
    {
        protected FrameworkPageViewModel(FrameworkPageBase page)
        {
            Page = page;
        }

        public FrameworkPageBase Page { get; }

        public IJSRuntime JS => Page.JS;
    }
}
