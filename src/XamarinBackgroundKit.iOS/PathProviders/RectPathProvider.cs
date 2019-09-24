using CoreGraphics;
using UIKit;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.iOS.PathProviders
{
    public class RectPathProvider : BasePathProvider<Rect>
    {
        public override bool IsBorderSupported => true;

        public override void CreatePath(Rect shape, CGRect bounds)
        {
            Path = UIBezierPath.FromRect(bounds).CGPath;
        }

        public override void CreateBorderedPath(Rect shape, CGRect bounds, double strokeWidth)
        {
            BorderPath = UIBezierPath.FromRect(bounds.Inset((float)strokeWidth, (float)strokeWidth)).CGPath;
        }
    }
}
