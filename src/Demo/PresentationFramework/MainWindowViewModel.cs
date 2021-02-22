using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.ViewModelUtils.Demo.PresentationFramework
{
    public sealed class MainWindowViewModel : WindowViewModel
    {
        private IEnumerable<CommandViewModelBase> _Commands;

        public IEnumerable<CommandViewModelBase> Commands
            => _Commands ??= CreateCommands().ToArray();

        private IEnumerable<CommandViewModelBase> CreateCommands()
        {
            yield return CommandViewModel.Create(
                () => new InteractionServiceWindow(new InteractionServiceWindowViewModel(new InteractionService()))
                {
                    Owner = Window
                }.Show(),
                title: "InteractionService");

            yield return CommandViewModel.Create(
                () => new InteractionServiceWindow(new InteractionServiceWindowViewModel(new FrameworkInteractionService()))
                {
                    Owner = Window
                }.Show(),
                title: "FrameworkInteractionService");
        }
    }
}
