using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Shipwreck.ViewModelUtils.Components
{
    public abstract class PopoverAnchor<T> : PopoverTarget<T>
        where T : class
    {
        [Parameter]
        public bool IsPrimary { get; set; }

        [Parameter]
        public int? TabIndex { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }

        protected virtual int? GetTabIndex() => TabIndex;
    }
}
