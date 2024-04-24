namespace Shipwreck.ViewModelUtils.Searching;

public partial class SearchPropertiesModalViewModel : FrameworkModalViewModelBase
{
    public SearchPropertiesModalViewModel(FrameworkPageViewModel page)
        : this(page, (ISearchPropertiesHost)page)
    {
    }

    public SearchPropertiesModalViewModel(FrameworkPageViewModel page, ISearchPropertiesHost host)
        : base(page)
    {
        Host = host;
    }

    public ISearchPropertiesHost Host { get; }

    public SearchPropertyGroupViewModel RootGroup => Host.RootGroup;

    public string Title => SR.AddSearchConditionTitle;

    protected override CommandViewModelBase CreateCancelCommand()
        => CommandViewModel.Create(
            () => Page.CloseModalAsync(this),
            title: SR.CloseTitle,
            style: BorderStyle.OutlineSecondary);
}
