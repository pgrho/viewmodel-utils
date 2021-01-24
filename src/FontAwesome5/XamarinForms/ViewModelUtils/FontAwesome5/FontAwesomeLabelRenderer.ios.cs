using Foundation;
using Shipwreck.ViewModelUtils.FontAwesome5;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FontAwesomeLabel), typeof(FontAwesomeLabelRenderer))]

namespace Shipwreck.ViewModelUtils.FontAwesome5
{
    public sealed class FontAwesomeLabelRenderer : LabelRenderer
    {
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            if (Element is FontAwesomeLabel c)
            {
                c.IsPressed = true;
            }
            base.TouchesBegan(touches, evt);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            if (Element is FontAwesomeLabel c)
            {
                c.IsPressed = false;
            }
            base.TouchesEnded(touches, evt);
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            if (Element is FontAwesomeLabel c)
            {
                c.IsPressed = false;
            }
            base.TouchesCancelled(touches, evt);
        }
    }
}
