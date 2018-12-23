using Android.Graphics.Drawables;
using static Android.Graphics.Drawables.GradientDrawable;

namespace XamarinBackgroundKit.Android.Extensions
{
    public static class GradientDrawableExtensions
    {
        public static void SetOrientation(this GradientDrawable drawable, float angle)
        {
            drawable.SetOrientation((double)angle);
        }

        public static void SetOrientation(this GradientDrawable drawable, double angle)
        {
            var orientation = angle >= 0 && angle < 45 ? Orientation.LeftRight :
                angle < 90 ? Orientation.BlTr :
                angle < 135 ? Orientation.BottomTop :
                angle < 180 ? Orientation.BrTl :
                angle < 225 ? Orientation.RightLeft :
                angle < 270 ? Orientation.TrBl :
                angle < 315 ? Orientation.TopBottom : Orientation.TlBr;

            drawable.SetOrientation(orientation);
        }
    }
}