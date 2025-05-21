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
    private bool _IsSaving;

    public bool IsAccepted
    {
        get => _IsAccepted;
        protected set => SetProperty(ref _IsAccepted, value);
    }

    public bool IsSaving
    {
        get => _IsSaving;
        protected set
        {
            if (SetProperty(ref _IsSaving, value))
            {
                _SaveCommand?.Invalidate();
                _SaveAndCloseCommand?.Invalidate();
            }
        }
    }

    private CommandViewModelBase _SaveCommand;
    private CommandViewModelBase _SaveAndCloseCommand;

    public CommandViewModelBase SaveCommand
        => _SaveCommand ??= GetSaveCommandBuilder().Build();

    public CommandViewModelBase SaveAndCloseCommand
        => _SaveAndCloseCommand ??= GetSaveAndCloseCommandBuilder().Build();

    protected virtual CommandBuilderBase GetSaveCommandBuilder()
        => new CommandBuilder(_ => BeginSave(false))
        .SetTitle(SR.SaveTitle)
        .SetMnemonic(SR.SaveMnemonic)
        .SetDescription(SR.SaveDescription.EmptyToNull())
        .SetStyle(BorderStyle.OutlineSecondary)
        .SetIsEnabled(_ => CanAccept)
        .SetIcon(c => c.IsExecuting || _IsSaving ? "fas fa-spinner fa-pulse" : "fas fa-save");

    protected virtual CommandBuilderBase GetSaveAndCloseCommandBuilder()
        => new CommandBuilder(_ => BeginSave(true))
        .SetTitle(SR.SaveAndCloseTitle)
        .SetMnemonic(SR.SaveAndCloseMnemonic)
        .SetDescription(SR.SaveAndCloseDescription.EmptyToNull())
        .SetStyle(BorderStyle.Primary)
        .SetIsEnabled(_ => CanAccept)
        .SetIcon(c => c.IsExecuting || _IsSaving ? "fas fa-spinner fa-pulse" : "fas fa-save");

    protected abstract void BeginSave(bool shouldClose);
}
