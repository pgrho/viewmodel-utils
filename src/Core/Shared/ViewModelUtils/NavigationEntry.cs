namespace Shipwreck.ViewModelUtils;

public abstract partial class NavigationEntry : ObservableModel
{
    protected NavigationEntry()
    { }

    public abstract string Path { get; }

    #region Title

    private string _Title;

    public string Title
    {
        get => _Title;
        set => SetProperty(ref _Title, value);
    }

    #endregion Title

    public virtual bool IsSupported => Path != null;

    public abstract FrameworkPageViewModel GetViewModel(IFrameworkPageViewModelArgs args, FrameworkPageViewModel current);
}
