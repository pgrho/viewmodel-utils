namespace Shipwreck.ViewModelUtils;

public abstract class CommandBuilderBase : ICommandViewModelHandler
{
    public string? Title { get; set; }
    public Func<CommandViewModelBase, string>? TitleGetter { get; set; }
    public string? Mnemonic { get; set; }
    public Func<CommandViewModelBase, string>? MnemonicGetter { get; set; }
    public string? Description { get; set; }
    public Func<CommandViewModelBase, string>? DescriptionGetter { get; set; }
    public bool? IsVisible { get; set; }
    public Func<CommandViewModelBase, bool>? IsVisibleGetter { get; set; }
    public bool? IsEnabled { get; set; }
    public Func<CommandViewModelBase, bool>? IsEnabledGetter { get; set; }
    public string? Icon { get; set; }
    public Func<CommandViewModelBase, string>? IconGetter { get; set; }
    public BorderStyle? Style { get; set; }
    public Func<CommandViewModelBase, BorderStyle>? StyleGetter { get; set; }
    public int? BadgeCount { get; set; }
    public Func<CommandViewModelBase, int>? BadgeCountGetter { get; set; }
    public string? Href { get; set; }
    public Func<CommandViewModelBase, string>? HrefGetter { get; set; }
    public Action<CommandViewModelBase>? ExecutingCallback { get; set; }
    public Action<CommandViewModelBase>? ExecutedCallback { get; set; }
    public ICommandViewModelHandler? Handler { get; set; }

    public abstract CommandViewModelBase Build();

    void ICommandViewModelHandler.OnCommandExecuting(CommandViewModelBase command)
    {
        Handler?.OnCommandExecuting(command);
        ExecutingCallback?.Invoke(command);
    }

    void ICommandViewModelHandler.OnCommandExecuted(CommandViewModelBase command)
    {
        ExecutedCallback?.Invoke(command);
        Handler?.OnCommandExecuted(command);
    }

    protected ICommandViewModelHandler? GetHandler()
        => ExecutingCallback != null
        || ExecutedCallback != null
        || Handler != null ? this : null;
}
