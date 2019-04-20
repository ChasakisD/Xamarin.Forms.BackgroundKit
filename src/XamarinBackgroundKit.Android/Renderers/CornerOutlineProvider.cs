using Android.Graphics;
using Android.Views;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class CornerOutlineProvider : ViewOutlineProvider
    {
        private readonly float _cornerRadius;

        public CornerOutlineProvider(float cornerRadius)
        {
            _cornerRadius = cornerRadius;
        }

        public override void GetOutline(View view, Outline outline)
        {
            outline.SetRoundRect(-1, 0, view.Width + 2, view.Height, _cornerRadius);
        }
    }
}