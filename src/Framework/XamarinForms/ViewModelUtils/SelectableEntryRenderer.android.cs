using System;
using System.ComponentModel;
using Android.Content;
using Shipwreck.ViewModelUtils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SelectableEntry), typeof(SelectableEntryRenderer))]
namespace Shipwreck.ViewModelUtils
{
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
                Control.SetSelectAllOnFocus((Element as SelectableEntry)?.SelectAllOnFocus ?? false);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null && Element != null)
            {
                switch (e.PropertyName)
                {
                    case nameof(SelectableEntry.CursorPosition):
                    case nameof(SelectableEntry.SelectionLength):
                        Control.RequestFocus();
                        Control.SetSelection(
                            Math.Max(0, Element.CursorPosition),
                            Math.Min(Element.Text?.Length ?? 0, Element.CursorPosition + Element.SelectionLength));
                        break;

                    case nameof(SelectableEntry.SelectAllOnFocus):
                        Control.SetSelectAllOnFocus((Element as SelectableEntry)?.SelectAllOnFocus ?? false);
                        break;
                }
            }
        }
    }
}
