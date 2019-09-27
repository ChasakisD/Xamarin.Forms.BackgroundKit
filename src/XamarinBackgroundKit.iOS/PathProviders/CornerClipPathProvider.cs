using CoreGraphics;
using UIKit;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.iOS.PathProviders
{
    public class CornerClipPathProvider : BasePathProvider<CornerClip>
    {
        public override bool IsBorderSupported => false;

        public override void CreatePath(CornerClip shape, CGRect bounds)
        {
            using (var bezierPath = new UIBezierPath())
            {
                var topLeftClipSize = (float)shape.ClipSize.TopLeft;
                var topRightClipSize = (float)shape.ClipSize.TopRight;
                var bottomLeftClipSize = (int)shape.ClipSize.BottomLeft;
                var bottomRightClipSize = (int)shape.ClipSize.BottomRight;

                bezierPath.MoveTo(new CGPoint(bounds.X + topLeftClipSize, bounds.Y));
                bezierPath.AddLineTo(new CGPoint(bounds.Width - topRightClipSize, bounds.Y));
                bezierPath.AddLineTo(new CGPoint(bounds.Width, bounds.X + topRightClipSize));
                bezierPath.AddLineTo(new CGPoint(bounds.Width, bounds.Height - bottomRightClipSize));
                bezierPath.AddLineTo(new CGPoint(bounds.Width - bottomRightClipSize, bounds.Height));
                bezierPath.AddLineTo(new CGPoint(bounds.X + bottomLeftClipSize, bounds.Height));
                bezierPath.AddLineTo(new CGPoint(bounds.X, bounds.Height - bottomLeftClipSize));
                bezierPath.AddLineTo(new CGPoint(bounds.X, bounds.Y + topLeftClipSize));
                bezierPath.AddLineTo(new CGPoint(bounds.X + topLeftClipSize, bounds.Y));
                bezierPath.ClosePath();

                Path = bezierPath.CGPath;
            }                
        }
    }
}
