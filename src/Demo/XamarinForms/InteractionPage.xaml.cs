using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InteractionPage : ContentPage
    {
        private class InteractionPageViewModel : FrameworkPageViewModel, IHasPage
        {
            public InteractionPageViewModel(Page page)
            {
                Page = page;
                Title = "Interaction";
            }

            public Page Page { get; }

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

        public InteractionPage()
        {
            InitializeComponent();
            BindingContext = new InteractionPageViewModel(this);
        }
    }
}
