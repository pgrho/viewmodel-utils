namespace Shipwreck.ViewModelUtils.Searching;

public partial class SearchPropertiesModalViewModel : FrameworkModalViewModelBase
{
    public SearchPropertiesModalViewModel(FrameworkPageViewModel page)
        : base(page)
    {
    }

    public IFrameworkSearchPageViewModel SearchPage => (IFrameworkSearchPageViewModel)Page;

    public string Title => SR.AddSearchConditionTitle;

    #region Groups

    private ReadOnlyCollection<SearchPropertyGroupViewModel> _Groups;

    public ReadOnlyCollection<SearchPropertyGroupViewModel> Groups
        => _Groups ??= Array.AsReadOnly(GetGroups().ToArray());

    protected virtual IEnumerable<SearchPropertyGroupViewModel> GetGroups()
        => (FrameworkPageViewModel.Handler as IFrameworkSearchPageViewModelHandler)?.CreatePropertyGroups(SearchPage)
            ?? SearchPage.Properties
                    .GroupBy(e => e.AncestorPath)
                    .OrderBy(e => e.Key ?? string.Empty)
                    .Select(g => new SearchPropertyGroupViewModel(this, g.Key, g));

    #endregion Groups

    #region AddParameterCommand

    private class AddCommand : ICommand
    {
        private readonly SearchPropertiesModalViewModel _Modal;

        public AddCommand(SearchPropertiesModalViewModel modal)
        {
            _Modal = modal;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }

        bool ICommand.CanExecute(object parameter) => parameter is SearchPropertyViewModel;

        public void Execute(object parameter)
        {
            if (parameter is SearchPropertyViewModel p)
            {
                _Modal.SearchPage.CreateOrGetCondition(p.Name);
            }
        }
    }

    private ICommand _AddParameterCommand;

    public ICommand AddParameterCommand
        => _AddParameterCommand ??= new AddCommand(this);

    #endregion AddParameterCommand

    protected override CommandViewModelBase CreateCancelCommand()
        => CommandViewModel.Create(
            () => Page.CloseModalAsync(this),
            title: SR.CloseTitle,
            style: BorderStyle.OutlineSecondary);
}
