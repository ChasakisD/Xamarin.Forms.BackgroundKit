using System;
using CoreGraphics;
using UIKit;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.iOS.PathProviders
{
    public class DiagonalPathProvider : BasePathProvider<Diagonal>
    {
        public override bool IsBorderSupported => false;

        public override void CreatePath(Diagonal shape, CGRect bounds)
        {
            var angleAbs = Math.Abs(shape.Angle);
            var isDirLeft = shape.Direction == ShapeDirection.Left;
            var diagonalHeight = (float)(bounds.Width * Math.Tan(Math.PI / 180 * angleAbs));

            using (var bezierPath = new UIBezierPath())
            {
                switch (shape.Position)
                {
                    case ShapePosition.Left:
                        if (isDirLeft)
                        {
                            bezierPath.MoveTo(new CGPoint(diagonalHeight, 0));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width, 0));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width, bounds.Height));
                            bezierPath.AddLineTo(new CGPoint(0, bounds.Height));
                        }
                        else
                        {
                            bezierPath.MoveTo(new CGPoint(0, 0));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width, 0));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width, bounds.Height));
                            bezierPath.AddLineTo(new CGPoint(diagonalHeight, bounds.Height));
                        }
                        break;
                    case ShapePosition.Top:
                        if (isDirLeft)
                        {
                            bezierPath.MoveTo(new CGPoint(bounds.Width, bounds.Height));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width, diagonalHeight));
                            bezierPath.AddLineTo(new CGPoint(0, 0));
                            bezierPath.AddLineTo(new CGPoint(0, bounds.Height));
                        }
                        else
                        {
                            bezierPath.MoveTo(new CGPoint(bounds.Width, bounds.Height));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width, 0));
                            bezierPath.AddLineTo(new CGPoint(0, diagonalHeight));
                            bezierPath.AddLineTo(new CGPoint(0, bounds.Height));
                        }
                        break;
                    case ShapePosition.Bottom:
                        if (isDirLeft)
                        {
                            bezierPath.MoveTo(new CGPoint(0, 0));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width, 0));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width, bounds.Height - diagonalHeight));
                            bezierPath.AddLineTo(new CGPoint(0, bounds.Height));
                        }
                        else
                        {
                            bezierPath.MoveTo(new CGPoint(bounds.Width, bounds.Height));
                            bezierPath.AddLineTo(new CGPoint(0, bounds.Height - diagonalHeight));
                            bezierPath.AddLineTo(new CGPoint(0, 0));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width, 0));
                        }
                        break;
                    case ShapePosition.Right:
                        if (isDirLeft)
                        {
                            bezierPath.MoveTo(new CGPoint(0, 0));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width, 0));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width - diagonalHeight, bounds.Height));
                            bezierPath.AddLineTo(new CGPoint(0, bounds.Height));
                        }
                        else
                        {
                            bezierPath.MoveTo(new CGPoint(0, 0));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width - diagonalHeight, 0));
                            bezierPath.AddLineTo(new CGPoint(bounds.Width, bounds.Height));
                            bezierPath.AddLineTo(new CGPoint(0, bounds.Height));
                        }
                        break;
                }

                bezierPath.ClosePath();

                Path = bezierPath.CGPath;
            }
        }
    }
}
