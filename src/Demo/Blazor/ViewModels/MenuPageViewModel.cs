namespace Shipwreck.ViewModelUtils.Demo.Blazor.ViewModels;

public sealed class MenuPageViewModel : PageViewModel
{
    public MenuPageViewModel(PageBase page)
        : base(page)
    {
    }

    public void OpenFile()
    => base.BeginDownload("/test.dat", openFile: true);

    public void DownloadFile()
        => base.BeginDownload("/test.png", openFile: false);

    public void NavigateToListTest()
        => Page?.NavigationManager?.NavigateTo("/listTest");
}
