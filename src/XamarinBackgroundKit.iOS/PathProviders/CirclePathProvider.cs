using System;
using CoreGraphics;
using UIKit;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.iOS.PathProviders
{
    public class CirclePathProvider : BasePathProvider<Circle>
    {
        public override bool IsBorderSupported => true;

        public override void CreatePath(Circle shape, CGRect bounds)
        {
            using (var bezierPath = GetCirclePath(bounds))
            {
                Path = bezierPath.CGPath;
            }
        }

        public override void CreateBorderedPath(Circle shape, CGRect bounds, double strokeWidth)
        {
            using (var bezierPath = GetCirclePath(bounds, (float)strokeWidth))
            {
                BorderPath = bezierPath.CGPath;
            }
        }

        private static UIBezierPath GetCirclePath(CGRect bounds, float borderWidth = 0f)
        {
            var startX = bounds.Left;
            var endX = bounds.Right;
            var startY = bounds.Top;
            var endY = bounds.Bottom;

            var delta = (float)Math.Abs(bounds.Width - bounds.Height) / 2f;

            if (bounds.Width > bounds.Height)
            {
                startX += delta;
                endX -= 2 * delta;
            }
            else if (bounds.Width < bounds.Height)
            {
                startY += delta;
                endY -= 2 * delta;
            }

            var croppedBounds = new CGRect(
                startX, startY, endX, endY);

            var strokedCroppedBounds = croppedBounds.Inset(
                borderWidth, borderWidth);

            var radius = ((float)Math.Min(bounds.Width, bounds.Height) - borderWidth) / 2;

            return UIBezierPath.FromRoundedRect(strokedCroppedBounds, radius);
        }
    }
}
