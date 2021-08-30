using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Shipwreck.ViewModelUtils.Components
{
    public partial class ConfirmModal : BootstrapModalBase<TaskCompletionSource<bool>>
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public string TrueText { get; set; } = SR.YesTitle;

        [Parameter]
        public BorderStyle TrueStyle { get; set; } = BorderStyle.Primary;

        [Parameter]
        public string FalseText { get; set; } = SR.NoTitle;

        [Parameter]
        public BorderStyle FalseStyle { get; set; } = BorderStyle.OutlineSecondary;

        protected override void OnIsOpenChanged()
        {
            base.OnIsOpenChanged();
            if (!IsOpen)
            {
                DataContext?.TrySetResult(false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            DataContext?.TrySetResult(false);
        }

        public static Task<bool> ShowAsync(
            IInteractionService interaction,
            ModalPresenterBase presenter,
            string message,
            string title = null,
            string trueText = null,
            BorderStyle? trueStyle = null,
            string falseText = null,
            BorderStyle? falseStyle = null)
        {
            if (interaction == null || presenter == null)
            {
                return Task.FromResult(false);
            }

            var tcs = new TaskCompletionSource<bool>();

            var props = new Dictionary<string, object>();

            props.Add(nameof(DataContext), tcs);
            props.Add(nameof(Message), message);
            props.Add(nameof(Title), title);
            props.Add(nameof(TrueText), trueText);
            props.Add(nameof(TrueStyle), trueStyle);
            props.Add(nameof(FalseText), falseText);
            props.Add(nameof(FalseStyle), falseStyle);
            props.Add(nameof(IsOpen), true);

            presenter.ShowModal(typeof(ConfirmModal), props.Where(e => e.Value != null));

            return tcs.Task;
        }
    }
}
