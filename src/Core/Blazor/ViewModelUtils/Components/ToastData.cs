namespace Shipwreck.ViewModelUtils.Components;

public partial class ToastData
{
    public ToastData(BorderStyle style, string title, string message, TimeSpan duration)
    {
        Style = style;
        Title = title;
        Message = message;
        Duration = duration;
    }

    public BorderStyle Style { get; }
    public string Title { get; }
    public string Message { get; }
    public TimeSpan Duration { get; }
}
