using System;
using System.Linq;
using Android.Graphics;
using XamarinBackgroundKit.Extensions;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.Android.PathProviders
{
    public class RoundRectPathProvider : BasePathProvider<RoundRect>
    {
        public float[] CornerRadii = new float[8];

        public override bool IsBorderSupported => true;

        public override bool CanHandledByOutline =>
            Math.Abs(CornerRadii.Sum() / CornerRadii.Length - CornerRadii[0]) < 0.0001;

        public override void CreatePath(Path path, RoundRect shape, int width, int height)
        {
            CornerRadii = shape.CornerRadius.ToRadii(BackgroundKit.Density);

            path.AddRoundRect(0, 0, width, height, CornerRadii, Path.Direction.Cw);
        }

        public override void CreateBorderedPath(Path path, RoundRect shape, int width, int height, int strokeWidth)
        {
            var cornerRadii = shape.CornerRadius.ToRadii(BackgroundKit.Density);

            for (var i = 0; i < cornerRadii.Length; i++)
                cornerRadii[i] -= cornerRadii[i] <= 0 ? 0 : strokeWidth;            

            path.AddRoundRect(strokeWidth, strokeWidth, width - strokeWidth, height - strokeWidth, cornerRadii, Path.Direction.Cw);
        }
    }
}
