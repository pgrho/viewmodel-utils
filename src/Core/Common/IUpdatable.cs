namespace Shipwreck.ViewModelUtils;

public interface IUpdatable
{
    bool IsUpdating { get; }

    event EventHandler UpdateCompleted;
}

