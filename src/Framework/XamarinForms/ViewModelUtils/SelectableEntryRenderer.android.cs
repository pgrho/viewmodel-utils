using Shipwreck.ViewModelUtils;

[assembly: ExportRenderer(typeof(SelectableEntry), typeof(SelectableEntryRenderer))]
namespace Shipwreck.ViewModelUtils;

public class SelectableEntryRenderer : EntryRenderer
{
    public SelectableEntryRenderer(Context context)
        : base(context)
    {
    }

    protected new SelectableEntry Element => (SelectableEntry)base.Element;

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (Control != null && Element != null)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectableEntry.CursorPosition):
                case nameof(SelectableEntry.SelectionLength):
                    if (Control.IsFocused)
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
            Control.ImeOptions = ImeAction.Done;
        }
    }
}
