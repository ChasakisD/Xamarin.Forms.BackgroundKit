using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.iOS.PathProviders
{
    public class TrianglePathProvider : BasePathProvider<Triangle>
    {
        public override bool IsBorderSupported => false;

        public override void CreatePath(Triangle shape, CGRect bounds)
        {
            using (var bezierPath = new UIBezierPath())
            {
                bezierPath.MoveTo(shape.PointA.ToCGPointProp(bounds));
                bezierPath.AddLineTo(shape.PointB.ToCGPointProp(bounds));
                bezierPath.AddLineTo(shape.PointC.ToCGPointProp(bounds));
                bezierPath.ClosePath();

                Path = bezierPath.CGPath;
            }            
        }
    }

    internal static class PointExtensions
    {
        public static CGPoint ToCGPointProp(this Point point, CGRect bounds)
        {
            return new CGPoint(point.X * bounds.Width, point.Y * bounds.Height);
        }
    }
}
