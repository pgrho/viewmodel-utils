using Android.Content;
using Android.Views;
using Shipwreck.ViewModelUtils.FontAwesome5;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FontAwesomeLabel), typeof(FontAwesomeLabelRenderer))]

namespace Shipwreck.ViewModelUtils.FontAwesome5
{
    public sealed class FontAwesomeLabelRenderer : LabelRenderer
    {
        public FontAwesomeLabelRenderer(Context context)
            : base(context)
        {
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    {
                        if (Element is FontAwesomeLabel c)
                        {
                            c.IsPressed = true;
                        }
                    }
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    {
                        if (Element is FontAwesomeLabel c)
                        {
                            c.IsPressed = false;
                        }
                    }
                    break;
            }

            return base.OnTouchEvent(e);
        }
    }
}
