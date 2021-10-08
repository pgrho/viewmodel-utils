using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Shipwreck.ViewModelUtils.Components
{
    public partial class CommandButton : BindableComponentBase<ICommandViewModel>
    {
        [Parameter]
        public string BaseClass { get; set; } = "btn";

        [Parameter]
        public BorderStyle? CommandStyle { get; set; }

        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int? BadgeCount { get; set; }

        [Parameter]
        public bool? IsVisible { get; set; }

        [Parameter]
        public bool? IsEnabled { get; set; }

        [Parameter]
        public bool? IsActive { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }

        [Parameter]
        public bool ShowIcon { get; set; } = true;

        [Parameter]
        public bool ShowTitle { get; set; } = true;

        [Parameter]
        public bool ShowBadge { get; set; } = true;

        [Parameter]
        public BorderStyle BadgeStyle { get; set; } = BorderStyle.Danger;
    }
}
