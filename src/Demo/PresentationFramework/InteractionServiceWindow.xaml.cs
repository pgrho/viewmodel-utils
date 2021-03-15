using Notifications.Wpf.Core.Controls;
using Shipwreck.ViewModelUtils.Controls;

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

        protected override FrameworkNotificationArea GetNotificationArea()
            => NotificationArea;
    }
}
