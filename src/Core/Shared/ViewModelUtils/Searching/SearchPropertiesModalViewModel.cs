namespace Shipwreck.ViewModelUtils.Searching;

public partial class SearchPropertiesModalViewModel : FrameworkModalViewModelBase
{
    public SearchPropertiesModalViewModel(FrameworkPageViewModel page)
        : this(page, (ISearchPropertiesHost)page)
    {
    }

    public SearchPropertiesModalViewModel(FrameworkPageViewModel page, ISearchPropertiesHost host)
        : base(page)
    {
        Host = host;
    }

    public ISearchPropertiesHost Host { get; }

    public SearchPropertyGroupViewModel RootGroup => Host.RootGroup;

    public string Title => SR.AddSearchConditionTitle;



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
                var c = p.CreateCondition();
                if (c != null)
                {
                    p.Host.Conditions.Add(c);
                }
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
