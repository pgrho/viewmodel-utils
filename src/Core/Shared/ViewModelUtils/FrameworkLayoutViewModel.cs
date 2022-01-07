namespace Shipwreck.ViewModelUtils;

public abstract class FrameworkLayoutViewModel : ObservableModel, IDisposable
{
    #region IDisposable

    protected bool IsDisposed { get; set; }

    protected virtual void Dispose(bool disposing)
    {
        IsDisposed = true;
    }

#pragma warning disable CA1063 // Implement IDisposable Correctly

    public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
            => Dispose(true);

    #endregion IDisposable
}
