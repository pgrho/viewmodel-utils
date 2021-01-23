namespace Shipwreck.ViewModelUtils.Demo.PresentationFramework
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            new BulkUpdateableCollection<object> { };
            new Bootstrap4.ColorScheme { };
            new FontAwesome5.FontAwesomeTextBlock { Icon = "fa fa-spinner" };
        }
    }
}
