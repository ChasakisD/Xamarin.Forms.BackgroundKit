using Android.Graphics;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.Android.PathProviders
{
    public class CornerClipPathProvider : BasePathProvider<CornerClip>
    {
        public override bool IsBorderSupported => false;

        public override void CreatePath(Path path, CornerClip shape, int width, int height)
        {
            var density = BackgroundKit.Density;
            var topLeftClipSizePx = (int)(shape.ClipSize.TopLeft * density);
            var topRightClipSizePx = (int)(shape.ClipSize.TopRight * density);
            var bottomLeftClipSizePx = (int)(shape.ClipSize.BottomLeft * density);
            var bottomRightClipSizePx = (int)(shape.ClipSize.BottomRight * density);

            path.MoveTo(topLeftClipSizePx, 0);
            path.LineTo(width - topRightClipSizePx, 0);
            path.LineTo(width, topRightClipSizePx);
            path.LineTo(width, height - bottomRightClipSizePx);
            path.LineTo(width - bottomRightClipSizePx, height);
            path.LineTo(bottomLeftClipSizePx, height);
            path.LineTo(0, height - bottomLeftClipSizePx);
            path.LineTo(0, topLeftClipSizePx);
            path.LineTo(topLeftClipSizePx, 0);
            path.Close();
        }
    }
}
