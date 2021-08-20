using System.Linq;

namespace Shipwreck.ViewModelUtils
{
    public partial class MenuPageGroupViewModel : ObservableModel
    {
        internal MenuPageGroupViewModel(string title)
        {
            Title = title;
            Items = new BulkUpdateableCollection<MenuPageItemViewModel>();
        }

        public string Title { get; }

        public BulkUpdateableCollection<MenuPageItemViewModel> Items { get; }

        #region IsVisible

        private bool _IsVisible;

        public bool IsVisible
        {
            get => _IsVisible;
            private set => SetProperty(ref _IsVisible, value);
        }

        #endregion IsVisible

        public MenuPageItemViewModel AddAction(CommandViewModelBase command, bool isExtension = false)
        {
            var item = new MenuPageItemViewModel(this, command, isExtension: isExtension);
            Items.Add(item);
            Invalidate();
            return item;
        }

        public MenuPageGroupViewModel Invalidate()
        {
            IsVisible = Items.Any(e => e.Command.IsVisible);
            return this;
        }
    }
}
