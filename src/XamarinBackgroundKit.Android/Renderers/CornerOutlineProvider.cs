using Android.Graphics;
using Android.Views;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class CornerOutlineProvider : ViewOutlineProvider
    {
        private float[] _cornerRadii;

        public CornerOutlineProvider(float[] cornerRadii)
        {
            SetCornerRadii(cornerRadii);
        }

        public void SetCornerRadii(float[] cornerRadii)
        {
            _cornerRadii = cornerRadii;
        }

        public override void GetOutline(View view, Outline outline)
        {
            var roundPath = new Path();
            roundPath.AddRoundRect(0, 0, view.Width, view.Height, _cornerRadii, Path.Direction.Cw);

            outline.SetConvexPath(roundPath);
        }
    }
}