using System;
using System.Linq;
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

        private bool IsUniform() => Math.Abs(_cornerRadii.Sum() / _cornerRadii.Length - _cornerRadii[0]) < 0.0001;

        public override void GetOutline(View view, Outline outline)
        {
            if (IsUniform())
            {
                outline.SetRoundRect(0, 0, view.Width, view.Height, _cornerRadii[0]);
                return;
            }

            using (var roundPath = new Path())
            {
                roundPath.AddRoundRect(0, 0, view.Width, view.Height, _cornerRadii, Path.Direction.Cw);
                outline.SetConvexPath(roundPath);
            }
        }
    }
}