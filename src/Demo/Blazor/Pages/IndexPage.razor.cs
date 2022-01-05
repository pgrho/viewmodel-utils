using Microsoft.AspNetCore.Components;
using Shipwreck.ViewModelUtils.Components;

namespace Shipwreck.ViewModelUtils.Demo.Blazor.Pages
{
    public partial class IndexPage : FrameworkPageBase
    {
        [Inject]
        public InteractionService Interaction { get; set; }

        public new IndexPageViewModel DataContext
        {
            get => (IndexPageViewModel)base.DataContext;
            set => base.DataContext = value;
        }

        protected override FrameworkPageViewModel GetOrCreateDataContext()
            => base.DataContext as IndexPageViewModel ?? new IndexPageViewModel(this);
    }

    public sealed class IndexPageViewModel : FrameworkPageViewModel
    {
        public IndexPageViewModel(FrameworkPageBase page)
            : base(page)
        {
        }

        public new IndexPage Page => (IndexPage)base.Page;

        protected override IInteractionService GetInteractionService()
            => Page?.Interaction;

        public void OpenFile()
            => base.BeginDownload("/test.dat", openFile: true);

        public void DownloadFile()
            => base.BeginDownload("/test.png", openFile: false);
    }
}
