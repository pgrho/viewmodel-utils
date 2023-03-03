using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms;

public class MainPageViewModel : PageViewModel
{
    public MainPageViewModel(Page page)
        : base(page)
    {
        Title = "Main";
    }

    #region Commands

    private CommandViewModelCollection _Commands;

    public CommandViewModelCollection Commands
        => _Commands ??= new CommandViewModelCollection(CreateCommands().ToArray());

    private IEnumerable<CommandViewModelBase> CreateCommands()
    {
        yield return CommandViewModel.CreateAsync(() => NavigateAsync(new InteractionPage()), title: "InteractionPage", style: BorderStyle.OutlinePrimary);
        yield return CommandViewModel.CreateAsync(() => NavigateAsync(new EntitySelectorPage()), title: "EntitySelectorPage", style: BorderStyle.OutlinePrimary);
    }

    private Task NavigateAsync(Page p) => Application.Current.MainPage.Navigation.PushAsync(p);

    #endregion Commands
}
