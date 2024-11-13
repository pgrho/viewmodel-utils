using System.Collections;

namespace Shipwreck.ViewModelUtils.Validation;

public abstract class ValidatableModel : ObservableModel, INotifyDataErrorInfo
{
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    #region INotifyDataErrorInfo

    private Dictionary<string, HashSet<string>>? _Errors;

    public Dictionary<string, HashSet<string>> Errors
        => _Errors ?? (_Errors = new Dictionary<string, HashSet<string>>());

    public bool HasErrors
        => Errors?.Count > 0;

    IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
    {
        if (Errors != null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return Errors.SelectMany(e => e.Value);
            }
            else if (Errors.TryGetValue(propertyName!, out var l))
            {
                return l.AsEnumerable();
            }
        }
        return Enumerable.Empty<string>();
    }

    protected internal bool AddError(string errorMessage, [CallerMemberName] string propertyName = null)
    {
        var ph = HasErrors;
        propertyName = propertyName ?? string.Empty;
        if (!Errors.TryGetValue(propertyName, out var l))
        {
            Errors[propertyName] = l = new HashSet<string>();
        }
        if (l.Add(errorMessage))
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

            if (!ph)
            {
                RaisePropertyChanged(nameof(HasErrors));
            }

            return true;
        }
        return false;
    }

    protected internal bool SetErrors(IEnumerable<string> errorMessages, [CallerMemberName] string propertyName = null)
    {
        propertyName = propertyName ?? string.Empty;
        if (errorMessages?.Any() == true)
        {
            var ph = HasErrors;
            if (!Errors.TryGetValue(propertyName, out var l))
            {
                Errors[propertyName] = l = new HashSet<string>();
            }
            if (l.Except(errorMessages).Any() || errorMessages.Except(l).Any())
            {
                l.Clear();
                l.UnionWith(errorMessages);

                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                if (!ph)
                {
                    RaisePropertyChanged(nameof(HasErrors));
                }
                return true;
            }

            return false;
        }
        else
        {
            return ClearError(propertyName);
        }
    }

    protected internal bool RemoveError(string errorMessage, [CallerMemberName] string propertyName = null)
    {
        var ph = HasErrors;
        propertyName = propertyName ?? string.Empty;

        if (_Errors != null
            && _Errors.TryGetValue(propertyName, out var l)
            && l.Remove(errorMessage))
        {
            if (!l.Any())
            {
                _Errors.Remove(propertyName);
            }
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            if (ph && !HasErrors)
            {
                RaisePropertyChanged(nameof(HasErrors));
            }
            return true;
        }
        return false;
    }

    protected internal bool ClearError([CallerMemberName] string propertyName = null)
    {
        var ph = HasErrors;
        propertyName = propertyName ?? string.Empty;

        if (_Errors != null
            && _Errors.TryGetValue(propertyName, out var l))
        {
            _Errors.Remove(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            if (ph && !HasErrors)
            {
                RaisePropertyChanged(nameof(HasErrors));
            }
            return true;
        }
        return false;
    }

    #endregion INotifyDataErrorInfo
}
