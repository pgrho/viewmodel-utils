namespace Shipwreck.ViewModelUtils;

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
        => new CommandBuilder(() => BeginSave(false))
        .SetTitle(SR.SaveTitle)
        .SetMnemonic(SR.SaveMnemonic)
        .SetDescription(SR.SaveDescription.EmptyToNull())
        .SetStyle(BorderStyle.OutlineSecondary)
        .SetIsEnabled(() => CanAccept);

    protected virtual CommandBuilderBase GetSaveAndCloseCommandBuilder()
        => new CommandBuilder(() => BeginSave(true))
        .SetTitle(SR.SaveAndCloseTitle)
        .SetMnemonic(SR.SaveAndCloseMnemonic)
        .SetDescription(SR.SaveAndCloseDescription.EmptyToNull())
        .SetStyle(BorderStyle.Primary)
        .SetIsEnabled(() => CanAccept);

    protected abstract void BeginSave(bool shouldClose);
}
