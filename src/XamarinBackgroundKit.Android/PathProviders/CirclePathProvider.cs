using System;
using Android.Graphics;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.Android.PathProviders
{
    public class CirclePathProvider : BasePathProvider<Circle>
    {
        public override bool IsBorderSupported => true;

        public override void CreatePath(Path path, Circle shape, int width, int height)
        {
            var radius = Math.Min(width, height) / 2f;

            path.AddCircle(width / 2f, height / 2f, radius, Path.Direction.Ccw);
        }

        public override void CreateBorderedPath(Path path, Circle shape, int width, int height, int strokeWidth)
        {
            var radius = (Math.Min(width, height) - strokeWidth) / 2f;

            path.AddCircle(width / 2f, height / 2f, radius, Path.Direction.Ccw);
        }
    }
}
