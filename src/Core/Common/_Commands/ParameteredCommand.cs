using System.Windows.Input;

namespace Shipwreck.ViewModelUtils;

public class ParameteredCommand : ICommand
{
    private readonly Action<object?> _Executed;
    private readonly Func<object?, bool>? _CanExecute;

    public ParameteredCommand(Action<object?> executed, Func<object?, bool>? canExecute = null)
    {
        _Executed = executed ?? throw new ArgumentNullException(nameof(executed));
        _CanExecute = canExecute;
    }

    event EventHandler? ICommand.CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object? parameter)
        => _CanExecute?.Invoke(parameter) != false;

    void ICommand.Execute(object? parameter)
        => _Executed(parameter);
}
public class ParameteredCommand<T> : ICommand
{
    private readonly Action<T?> _Executed;
    private readonly Func<T?, bool>? _CanExecute;

    public ParameteredCommand(Action<T?> executed, Func<T?, bool>? canExecute = null)
    {
        _Executed = executed ?? throw new ArgumentNullException(nameof(executed));
        _CanExecute = canExecute;
    }

    event EventHandler? ICommand.CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object? parameter)
    {
        if (_CanExecute == null)
        {
            return true;
        }
        if (parameter is T p)
        {
            return _CanExecute(p);
        }
        else if (!typeof(T).IsValueType || Nullable.GetUnderlyingType(typeof(T)) != null)
        {
            return _CanExecute(default);
        }
        return false;
    }

    void ICommand.Execute(object? parameter)
        => _Executed(parameter is T p ? p : default);
}
