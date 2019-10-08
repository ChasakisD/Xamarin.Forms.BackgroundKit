using Android.Graphics;
using Rect = XamarinBackgroundKit.Shapes.Rect;

namespace XamarinBackgroundKit.Android.PathProviders
{
    public class RectPathProvider : BasePathProvider<Rect>
    {
        public override bool IsBorderSupported => true;

        public override void CreatePath(Path path, Rect shape, int width, int height)
        {
            path.AddRect(0, 0, width, height, Path.Direction.Cw);
        }

        public override void CreateBorderedPath(Path path, Rect shape, int width, int height, int strokeWidth)
        {
            path.AddRect(strokeWidth, strokeWidth, width - strokeWidth, height - strokeWidth, Path.Direction.Cw);
        }
    }
}

