using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms;

public abstract class PageViewModel : FrameworkPageViewModel, IHasPage
{
    private protected PageViewModel(Page page)
    {
        Page = page;
    }

    public Page Page { get; }
}
