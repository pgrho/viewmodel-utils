using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms
{
    public partial class MainPage : ContentPage
    {
        private class MainPageViewModel : FrameworkPageViewModel, IHasPage
        {
            public MainPageViewModel(Page page)
            {
                Page = page;
                Title = "Main";
            }
            public Page Page { get; }

            #region Commands

            private CommandViewModelCollection _Commands;

            public CommandViewModelCollection Commands
                => _Commands ??= new CommandViewModelCollection(CreateCommands().ToArray());

            private IEnumerable<CommandViewModelBase> CreateCommands()
            {
                yield return CommandViewModel.CreateAsync(() => NavigateAsync(new InteractionPage()), title: "InteractionPage", style: BorderStyle.OutlinePrimary);
            }

            private Task NavigateAsync(Page p) => Application.Current.MainPage.Navigation.PushAsync(p);

            #endregion Commands
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(this);
        }
    }
}
