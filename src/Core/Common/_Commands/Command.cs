using System.Windows.Input;

namespace Shipwreck.ViewModelUtils;

public sealed class Command : ICommand
{
    private readonly Action _Executed;

    public Command(Action executed)
    {
        _Executed = executed;
    }

    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object? parameter)
        => true;

    public void Execute(object? parameter)
        => _Executed();

    #region ファクトリーメソッド

    public static Command Create(Action executed)
        => new Command(executed);

    public static ParameteredCommand Create(Action<object?> executed, Func<object?, bool>? canExecute = null)
        => new ParameteredCommand(executed, canExecute);

    public static ParameteredCommand<T> Create<T>(Action<T?> executed, Func<T?, bool>? canExecute = null)
        => new ParameteredCommand<T>(executed, canExecute);

    public static DisableableCommand Create(Action executed, bool isEnabled)
        => new DisableableCommand(executed) { IsEnabled = isEnabled };

    #endregion ファクトリーメソッド
}
