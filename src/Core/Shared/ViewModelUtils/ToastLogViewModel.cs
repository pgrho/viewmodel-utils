namespace Shipwreck.ViewModelUtils;

public sealed partial class ToastLogViewModel
{
    internal ToastLogViewModel(FrameworkPageViewModel page, BorderStyle style, string message, string title)
    {
        Page = page;
        Style = style;
        Message = message;
        Title = title;
        Timestamp = DateTimeOffset.Now;
    }

    public FrameworkPageViewModel Page { get; }
    public BorderStyle Style { get; }
    public string Message { get; }

    public string Title { get; }

    public DateTimeOffset Timestamp { get; }
}
