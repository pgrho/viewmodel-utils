using System;
using System.Windows.Input;

namespace Shipwreck.ViewModelUtils
{
    public static class ViewModelCommands
    {
        private sealed class ToggleIsExpandedCommand : ICommand
        {
            event EventHandler ICommand.CanExecuteChanged
            {
                add { }
                remove { }
            }

            bool ICommand.CanExecute(object parameter)
                => (parameter as IExpandable)?.IsExpandable == true;

            void ICommand.Execute(object parameter)
            {
                if (parameter is IExpandable e)
                {
                    e.IsExpanded = !e.IsExpanded && e.IsExpandable;
                }
            }
        }

        public static ICommand ToggleIsExpanded { get; } = new ToggleIsExpandedCommand();
    }
}
