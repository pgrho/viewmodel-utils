using Shipwreck.ViewModelUtils;

[assembly: ExportRenderer(typeof(SelectableEntry), typeof(SelectableEntryRenderer))]
namespace Shipwreck.ViewModelUtils;

public class SelectableEntryRenderer : EntryRenderer
{
    public SelectableEntryRenderer(Context context)
        : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
        base.OnElementChanged(e);

        if (Control != null)
        {
            Control.ShowSoftInputOnFocus = (Element as SelectableEntry)?.IsKeyboardEnabled ?? true;
        }
    }

    private bool _IsFocusing;

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (Control != null && Element != null)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectableEntry.IsFocused):
                    if (Element.IsFocused
                        && (Element as SelectableEntry)?.SelectAllOnFocus == true
                        && Control.RequestFocus())
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            try
                            {
                                _IsFocusing = true;

                                ((SelectableEntry)Element).CursorPosition = 0;
                                ((SelectableEntry)Element).SelectionLength = Element.Text?.Length ?? 0;
                                Control.SelectAll();
                            }
                            finally
                            {
                                _IsFocusing = false;
                            }
                        });
                    }
                    return;

                case nameof(SelectableEntry.CursorPosition):
                case nameof(SelectableEntry.SelectionLength):

                    if (Control.IsFocused && !_IsFocusing)
                    {
                        var start = Math.Max(0, Element.CursorPosition);
                        var stop = Math.Min(Element.Text?.Length ?? 0, Element.CursorPosition + Element.SelectionLength);
                        Control.SetSelection(start, stop);
                    }

                    return;
            }
        }

        base.OnElementPropertyChanged(sender, e);

        if (Control != null && Element != null)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectableEntry.IsKeyboardEnabled):
                    Control.ShowSoftInputOnFocus = (Element as SelectableEntry)?.IsKeyboardEnabled ?? true;
                    break;
            }
        }
    }
}
