namespace Shipwreck.ViewModelUtils.Demo.Maui;

public abstract class PageViewModel : FrameworkPageViewModel, IHasPage
{
    private protected PageViewModel(Page page)
    {
        Page = page;
    }

    public Page Page { get; }
}
