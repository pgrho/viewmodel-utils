using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Shipwreck.ViewModelUtils.Components
{
    public partial class StickyTable<T> : ListComponentBase<T>
        where T : class
    {
        private ElementReference _Element;

        public ElementReference Container => _Element;

        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }

        [Parameter]
        public RenderFragment FooterTemplate { get; set; }

        [Parameter]
        public RenderFragment<ItemTemplateContext<T>> ItemTemplate { get; set; }

        [Parameter]
        public ISortablePageViewModel SearchPage { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }

        [Parameter]
        public string TableClass { get; set; } = "table table-sm table-hover";

        [Parameter]
        public string TheadClass { get; set; } = "thead-dark";
    }
}
