using Android.Graphics;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.Android.PathProviders
{
    public class TrianglePathProvider : BasePathProvider<Triangle>
    {
        public override bool IsBorderSupported => false;

        public override void CreatePath(Path path, Triangle shape, int width, int height)
        {
            path.MoveTo((float)shape.PointA.X * width, (float)shape.PointA.Y * height);
            path.LineTo((float)shape.PointB.X * width, (float)shape.PointB.Y * height);
            path.LineTo((float)shape.PointC.X * width, (float)shape.PointC.Y * height);
            path.Close();
        }
    }
}
