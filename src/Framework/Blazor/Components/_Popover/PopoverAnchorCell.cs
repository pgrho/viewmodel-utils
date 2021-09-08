using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.AspNetCore.Components;

namespace Shipwreck.ViewModelUtils.Components
{
    public abstract class PopoverAnchorCell<T> : BindableComponentBase<T>
        where T : class
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool IsPrimary { get; set; } = true;

        [Parameter]
        [Obsolete]
        public bool IsDark
        {
            get => !IsPrimary;
            set => IsPrimary = !value;
        }

        [Parameter]
        public ICommand Command { get; set; }

        [Parameter]
        public PopoverTargetCommandMode CommandMode { get; set; } = PopoverTargetCommandMode.Replace;

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }
    }
}
