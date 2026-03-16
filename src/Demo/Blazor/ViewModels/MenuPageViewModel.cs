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
    public void NavigateToButtonTest()
        => Page?.NavigationManager?.NavigateTo("/buttonTest");

    private CommandViewModelBase _OpenModalCommand;

    public CommandViewModelBase OpenModalCommand => _OpenModalCommand
        ??= CommandViewModel.CreateAsync(
              async _ =>
              {
                  try
                  {
                      await OpenModalAsync(new TestModalViewModel(this));
                  }
                  catch (Exception ex)
                  {
                      LogError(ex.ToString());
                      await ShowErrorToastAsync(ex.Message);
                  }
              }, title: "Open Modal");
}
