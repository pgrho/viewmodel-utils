using System;

namespace Shipwreck.ViewModelUtils
{
    public partial class FrameworkPageViewModel
    {
        public struct ProcessingDisabled : IDisposable
        {
            public void Dispose()
            {
            }
        }

        public ProcessingDisabled DisableProcessing()
            => default;
    }
}
