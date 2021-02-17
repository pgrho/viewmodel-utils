using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Shipwreck.ViewModelUtils
{
    public sealed class DisableableCommand : ICommand, INotifyPropertyChanged
    {
        private readonly Action _Executed;

        private bool _IsEnabled;

        public DisableableCommand(Action executed, bool isEnabled = true)
        {
            _Executed = executed;
            _IsEnabled = isEnabled;
        }

        public bool IsEnabled
        {
            get => _IsEnabled;
            set
            {
                if (value != _IsEnabled)
                {
                    _IsEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void SetIsEnabled(bool value)
            => IsEnabled = value;

        public event EventHandler CanExecuteChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanExecute(object parameter)
            => _IsEnabled;

        public void Execute(object parameter)
            => _Executed();
    }
}
