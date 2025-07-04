namespace Shipwreck.ViewModelUtils.Components;

public abstract class ToastPresenterBase : ListComponentBase<ToastData>
{
    protected ToastPresenterBase()
    {
        Source = new BulkUpdateableCollection<ToastData>();
    }

    [Parameter]
    public int MaximumCount { get; set; } = int.MaxValue;

    [Parameter]
    public Action OnAdded { get; set; }

    [Parameter]
    public int Duration { get; set; } = 3000;

    public void Add(BorderStyle style, string message, string title, TimeSpan? duration = null)
    {
        var mc = Math.Max(0, MaximumCount - 1);
        while (Source.Count > mc)
        {
            Source.RemoveAt(Source.Count - 1);
        }
        Source.Insert(
            0,
            new ToastData(style, title, message, duration ?? TimeSpan.FromMilliseconds(Duration)));
        OnAdded?.Invoke();
    }
}
