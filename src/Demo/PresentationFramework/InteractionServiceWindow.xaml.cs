namespace Shipwreck.ViewModelUtils.Demo.PresentationFramework
{
    /// <summary>
    /// InteractionServiceWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class InteractionServiceWindow
    {
        public InteractionServiceWindow(InteractionServiceWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
