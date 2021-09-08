using System;
using Microsoft.AspNetCore.Components; 

namespace Shipwreck.ViewModelUtils.Components
{
    public partial class DropdownButton : ListComponentBase<ICommandViewModel>
    {
        private static int _NewId;
        private string _Id = "DropdownButton--" + (++_NewId);

        [Parameter]
        public string BaseClass { get; set; } = "btn";

        [Parameter]
        [Obsolete]
        public BorderStyle? Type
        {
            get => Style;
            set => Style = value;
        }

        [Parameter]
        public BorderStyle? Style { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Description { get; set; }
    }
}
