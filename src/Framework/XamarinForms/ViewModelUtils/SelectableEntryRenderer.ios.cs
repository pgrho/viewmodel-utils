using System;
using System.ComponentModel;
using Shipwreck.ViewModelUtils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SelectableEntry), typeof(SelectableEntryRenderer))]
namespace Shipwreck.ViewModelUtils
{
    public class SelectableEntryRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null
                && Element != null
                && (e.PropertyName == nameof(SelectableEntry.CursorPosition) || e.PropertyName == nameof(SelectableEntry.SelectionLength)))
            {
                Control.SelectedTextRange = Control.GetTextRange(
                    Control.GetPosition(Control.BeginningOfDocument, Math.Max(0, Element.CursorPosition)),
                    Control.GetPosition(Control.BeginningOfDocument, Math.Min(Element.Text?.Length ?? 0, Element.CursorPosition + Element.SelectionLength)));
            }
        }
    }
}
