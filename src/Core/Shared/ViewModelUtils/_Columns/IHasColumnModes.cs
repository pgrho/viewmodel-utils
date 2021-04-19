using System.Collections.Generic;

namespace Shipwreck.ViewModelUtils
{
    public interface IHasColumnModes : IHasColumns
    {
        IReadOnlyList<SelectionCommandViewModelBase> ModeCommands { get; }

        void OnExtensionsCreated(IEnumerable<ExtensionColumnCommandViewModel> commands);
    }
}
