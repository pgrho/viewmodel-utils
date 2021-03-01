namespace Shipwreck.ViewModelUtils
{
    public abstract class DialogViewModel : WindowViewModel
    {
        private bool _CanAccept;

        public bool CanAccept
        {
            get => _CanAccept;
            protected set
            {
                if (SetProperty(ref _CanAccept, value))
                {
                    _SaveCommand?.Invalidate();
                    _SaveAndCloseCommand?.Invalidate();
                }
            }
        }

        private bool _IsAccepted;

        public bool IsAccepted
        {
            get => _IsAccepted;
            protected set => SetProperty(ref _IsAccepted, value);
        }

        private CommandViewModelBase _SaveCommand;
        private CommandViewModelBase _SaveAndCloseCommand;

        public CommandViewModelBase SaveCommand
            => _SaveCommand ??= GetSaveCommandBuilder().Build();

        public CommandViewModelBase SaveAndCloseCommand
            => _SaveAndCloseCommand ??= GetSaveAndCloseCommandBuilder().Build();

        protected virtual CommandBuilderBase GetSaveCommandBuilder()
            => new CommandBuilder()
            {
                ExecutionHandler = () => BeginSave(false)
            }
            .SetTitle("適用")
            .SetStyle(BorderStyle.OutlineSecondary)
            .SetIsEnabled(() => CanAccept);

        protected virtual CommandBuilderBase GetSaveAndCloseCommandBuilder()
            => new CommandBuilder()
            {
                ExecutionHandler = () => BeginSave(true)
            }
            .SetTitle("OK")
            .SetStyle(BorderStyle.Primary)
            .SetIsEnabled(() => CanAccept);

        protected abstract void BeginSave(bool shouldClose);
    }
}
