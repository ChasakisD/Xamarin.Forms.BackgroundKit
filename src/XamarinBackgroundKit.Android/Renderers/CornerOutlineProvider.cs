using Android.Graphics;
using Android.Views;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class CornerOutlineProvider : ViewOutlineProvider
    {
        private readonly float[] _cornerRadii;

        public CornerOutlineProvider(float[] cornerRadii)
        {
            _cornerRadii = cornerRadii;
        }

        public override void GetOutline(View view, Outline outline)
        {
            var roundPath = new Path();
            roundPath.AddRoundRect(-1, 0, view.Width + 2, view.Height, _cornerRadii, Path.Direction.Cw);

            outline.SetConvexPath(roundPath);
        }
    }
}