using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms;

public class InteractionPageViewModel : PageViewModel
{
    public InteractionPageViewModel(Page page)
        : base(page)
    {
        Title = "Interaction";
    }

    #region Commands

    private CommandViewModelCollection _Commands;

    public CommandViewModelCollection Commands
        => _Commands ??= new CommandViewModelCollection(CreateCommands().ToArray());

    private IEnumerable<CommandViewModelBase> CreateCommands()
    {
        yield return CommandViewModel.CreateAsync(() => AlertAsync("AlertAsync"), title: "AlertAsync", style: BorderStyle.OutlinePrimary);
        yield return CommandViewModel.CreateAsync(async () =>
        {
            var r = await ConfirmAsync("ConfirmAsync");
            await AlertAsync(r.ToString());
        }, title: "ConfirmAsync", style: BorderStyle.OutlinePrimary);
        yield return CommandViewModel.CreateAsync(() => ShowSuccessToastAsync("ShowSuccessToastAsync"), title: "Success Toast", style: BorderStyle.OutlinePrimary);
        yield return CommandViewModel.CreateAsync(() => ShowErrorToastAsync("ShowErrorToastAsync"), title: "Error Toast", style: BorderStyle.OutlinePrimary);
    }

    #endregion Commands
}
