using System;
using CoreGraphics;
using UIKit;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.iOS.PathProviders
{
    public class ArcPathProvider : BasePathProvider<Arc>
    {
        public override bool IsBorderSupported => false;

        public override void CreatePath(Arc shape, CGRect bounds)
        {
            using (var path = new UIBezierPath())
            {
                var arcHeightAbs = Math.Abs(shape.ArcHeight);

                switch (shape.Position)
                {
                    case ShapePosition.Left:
                        if (shape.IsCropInside)
                        {
                            path.MoveTo(new CGPoint(bounds.Width, 0));
                            path.AddLineTo(new CGPoint(0, 0));
                            path.AddQuadCurveToPoint(new CGPoint(0, bounds.Height), new CGPoint(arcHeightAbs * 2, bounds.Height / 2));
                            path.AddLineTo(new CGPoint(bounds.Width, bounds.Height));
                            path.ClosePath();
                        }
                        else
                        {
                            path.MoveTo(new CGPoint(bounds.Width, 0));
                            path.AddLineTo(new CGPoint(arcHeightAbs, 0));
                            path.AddQuadCurveToPoint(new CGPoint(arcHeightAbs, bounds.Height), new CGPoint(-arcHeightAbs, bounds.Height / 2));
                            path.AddLineTo(new CGPoint(bounds.Width, bounds.Height));
                            path.ClosePath();
                        }
                        break;
                    case ShapePosition.Top:
                        if (shape.IsCropInside)
                        {
                            path.MoveTo(new CGPoint(0, bounds.Height));
                            path.AddLineTo(new CGPoint(0, 0));
                            path.AddQuadCurveToPoint(new CGPoint(bounds.Width, 0), new CGPoint(bounds.Width / 2, 2 * arcHeightAbs));
                            path.AddLineTo(new CGPoint(bounds.Width, bounds.Height));
                            path.ClosePath();
                        }
                        else
                        {
                            path.MoveTo(new CGPoint(0, arcHeightAbs));
                            path.AddQuadCurveToPoint(new CGPoint(bounds.Width, arcHeightAbs), new CGPoint(bounds.Width / 2, -arcHeightAbs));
                            path.AddLineTo(new CGPoint(bounds.Width, bounds.Height));
                            path.AddLineTo(new CGPoint(0, bounds.Height));
                            path.ClosePath();
                        }
                        break;
                    case ShapePosition.Right:
                        if (shape.IsCropInside)
                        {
                            path.MoveTo(new CGPoint(0, 0));
                            path.AddLineTo(new CGPoint(bounds.Width, 0));
                            path.AddQuadCurveToPoint(new CGPoint(bounds.Width, bounds.Height), new CGPoint(bounds.Width - arcHeightAbs * 2, bounds.Height / 2));
                            path.AddLineTo(new CGPoint(0, bounds.Height));
                            path.ClosePath();
                        }
                        else
                        {
                            path.MoveTo(new CGPoint(0, 0));
                            path.AddLineTo(new CGPoint(bounds.Width - arcHeightAbs, 0));
                            path.AddQuadCurveToPoint(new CGPoint(bounds.Width - arcHeightAbs, bounds.Height), new CGPoint(bounds.Width + arcHeightAbs, bounds.Height / 2));
                            path.AddLineTo(new CGPoint(0, bounds.Height));
                            path.ClosePath();
                        }
                        break;
                    case ShapePosition.Bottom:
                        if (shape.IsCropInside)
                        {
                            path.MoveTo(new CGPoint(0, 0));
                            path.AddLineTo(new CGPoint(0, bounds.Height));
                            path.AddQuadCurveToPoint(new CGPoint(bounds.Width, bounds.Height), new CGPoint(bounds.Width / 2, bounds.Height - 2 * arcHeightAbs));
                            path.AddLineTo(new CGPoint(bounds.Width, 0));
                            path.ClosePath();
                        }
                        else
                        {
                            path.MoveTo(new CGPoint(0, 0));
                            path.AddLineTo(new CGPoint(0, bounds.Height - arcHeightAbs));
                            path.AddQuadCurveToPoint(new CGPoint(bounds.Width, bounds.Height - arcHeightAbs), new CGPoint(bounds.Width / 2, bounds.Height + arcHeightAbs));
                            path.AddLineTo(new CGPoint(bounds.Width, 0));
                            path.ClosePath();
                        }
                        break;
                }

                Path = path.CGPath;
            }            
        }
    }
}
