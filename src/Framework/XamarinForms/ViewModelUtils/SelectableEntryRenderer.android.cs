using Shipwreck.ViewModelUtils;
using Application = Android.App.Application;

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

    protected new SelectableEntry Element => (SelectableEntry)base.Element;
    private bool _IsFocusing;

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (Control != null && Element != null)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectableEntry.IsFocused):
                    if (Element.IsFocused
                        && Element?.SelectAllOnFocus == true
                        && Control.RequestFocus())
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            try
                            {
                                _IsFocusing = true;

                                Element.CursorPosition = 0;
                                Element.SelectionLength = Element.Text?.Length ?? 0;
                                Control.SelectAll();

                                HideSoftInput();
                            }
                            finally
                            {
                                _IsFocusing = false;
                            }
                        });
                    }
                    else
                    {
                        HideSoftInput();
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
                    Control.ShowSoftInputOnFocus = Element?.IsKeyboardEnabled ?? true;
                    HideSoftInput();
                    break;

                case nameof(SelectableEntry.IsFocused):
                    HideSoftInput();
                    break;
            }
        }
    }

    private void HideSoftInput()
    {
        try
        {
            if (!Control.ShowSoftInputOnFocus && Control.IsFocused)
            {
                if (Application.Context?.GetSystemService(Context.InputMethodService) is InputMethodManager imm)
                {
                    imm.HideSoftInputFromWindow(Control.WindowToken, HideSoftInputFlags.None);
                }
            }
        }
        catch { }
    }
}
