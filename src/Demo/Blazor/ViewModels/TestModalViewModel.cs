namespace Shipwreck.ViewModelUtils.Demo.Blazor.ViewModels;

public sealed class TestModalViewModel : FrameworkModalViewModelBase
{
    internal TestModalViewModel(FrameworkPageViewModel page)
        : base(page)
    {
        OpenedAt = DateTime.Now;
    }

    public DateTime OpenedAt { get; }

    private CommandViewModelBase _OpenCommand;

    public CommandViewModelBase OpenCommand => _OpenCommand
        ??= CommandViewModel.CreateAsync(
              async _ =>
              {
                  try
                  {
                      await Page.OpenModalAsync(new TestModalViewModel(Page));
                  }
                  catch (Exception ex)
                  {
                      LogError(ex.ToString());
                      await ShowErrorToastAsync(ex.Message);
                  }
              }, title: "Open New");
}
