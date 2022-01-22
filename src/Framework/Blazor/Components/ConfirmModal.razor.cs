namespace Shipwreck.ViewModelUtils.Components
{
    public partial class ConfirmModal : BootstrapModalBase<TaskCompletionSource<bool>>
    {
        #region Title

        private string _Title;

        [Parameter]
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        #endregion Title

        #region Message

        private string _Message;

        [Parameter]
        public string Message
        {
            get => _Message;
            set => SetProperty(ref _Message, value);
        }

        #endregion Message

        #region TrueText

        private string _TrueText = SR.YesTitle;

        [Parameter]
        public string TrueText
        {
            get => _TrueText;
            set => SetProperty(ref _TrueText, value);
        }

        #endregion TrueText

        #region TrueStyle

        private BorderStyle _TrueStyle = BorderStyle.Primary;

        [Parameter]
        public BorderStyle TrueStyle
        {
            get => _TrueStyle;
            set => SetProperty(ref _TrueStyle, value);
        }

        #endregion TrueStyle

        #region FalseText

        private string _FalseText = SR.NoTitle;

        [Parameter]
        public string FalseText
        {
            get => _FalseText;
            set => SetProperty(ref _FalseText, value);
        }

        #endregion FalseText

        #region FalseStyle

        private BorderStyle _FalseStyle = BorderStyle.OutlineSecondary;

        [Parameter]
        public BorderStyle FalseStyle
        {
            get => _FalseStyle;
            set => SetProperty(ref _FalseStyle, value);
        }

        #endregion FalseStyle

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
