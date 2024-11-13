namespace Shipwreck.ViewModelUtils;

public abstract class ObservableModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty(ref string? field, string? value, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        if (value != field)
        {
            field = value;
            if (propertyName != null)
            {
                RaisePropertyChanged(propertyName);
            }
            onChanged?.Invoke();
            return true;
        }
        return false;
    }

    protected bool SetProperty<T>(ref T field, T value, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        if (!((field as IEquatable<T>)?.Equals(value) ?? Equals(field, value)))
        {
            field = value;
            if (propertyName != null)
            {
                RaisePropertyChanged(propertyName);
            }
            onChanged?.Invoke();
            return true;
        }
        return false;
    }

    protected bool SetProperty<T>(ref PropertyStore<T> field, T value, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        if (!((field.CurrentValue as IEquatable<T>)?.Equals(value) ?? Equals(field.CurrentValue, value)))
        {
            field.SetCurrentValue(value);
            if (propertyName != null)
            {
                RaisePropertyChanged(propertyName);
            }
            onChanged?.Invoke();
            return true;
        }
        return false;
    }

    protected bool SetFlagProperty(ref byte field, byte flag, bool hasFlag, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        var nv = (byte)(hasFlag ? (field | flag) : (field & ~flag));
        return SetProperty(ref field, nv, onChanged: onChanged, propertyName: propertyName);
    }

    protected bool SetFlagProperty(ref ushort field, ushort flag, bool hasFlag, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        var nv = (ushort)(hasFlag ? (field | flag) : (field & ~flag));
        return SetProperty(ref field, nv, onChanged: onChanged, propertyName: propertyName);
    }

    protected bool SetFlagProperty(ref uint field, uint flag, bool hasFlag, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        var nv = hasFlag ? (field | flag) : (field & ~flag);
        return SetProperty(ref field, nv, onChanged: onChanged, propertyName: propertyName);
    }

    protected bool SetFlagProperty(ref ulong field, ulong flag, bool hasFlag, Action? onChanged = null, [CallerMemberName] string? propertyName = null)
    {
        var nv = hasFlag ? (field | flag) : (field & ~flag);
        return SetProperty(ref field, nv, onChanged: onChanged, propertyName: propertyName);
    }
}
