using System;
using Android.Graphics;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.Android.PathProviders
{
    public class ArcPathProvider : BasePathProvider<Arc>
    {
        public override bool IsBorderSupported => false;

        public override void CreatePath(Path path, Arc shape, int width, int height)
        {
            var arcHeightAbs = (float) Math.Abs(BackgroundKit.Density * shape.ArcHeight);

            switch(shape.Position)
            {
                case ShapePosition.Left:
                    if (shape.IsCropInside)
                    {
                        path.MoveTo(width, 0);
                        path.LineTo(0, 0);
                        path.QuadTo(arcHeightAbs * 2, height / 2f, 0, height);
                        path.LineTo(width, height);
                        path.Close();
                    }
                    else
                    {
                        path.MoveTo(width, 0);
                        path.LineTo(arcHeightAbs, 0);
                        path.QuadTo(-arcHeightAbs, height / 2f, arcHeightAbs, height);
                        path.LineTo(width, height);
                        path.Close();
                    }
                    break;
                case ShapePosition.Top:
                    if (shape.IsCropInside)
                    {
                        path.MoveTo(0, height);
                        path.LineTo(0, 0);
                        path.QuadTo(width / 2f, 2 * arcHeightAbs, width, 0);
                        path.LineTo(width, height);
                        path.Close();
                    }
                    else
                    {
                        path.MoveTo(0, arcHeightAbs);
                        path.QuadTo(width / 2f, -arcHeightAbs, width, arcHeightAbs);
                        path.LineTo(width, height);
                        path.LineTo(0, height);
                        path.Close();
                    }
                    break;
                case ShapePosition.Right:
                    if (shape.IsCropInside)
                    {
                        path.MoveTo(0, 0);
                        path.LineTo(width, 0);
                        path.QuadTo(width - arcHeightAbs * 2, height / 2f, width, height);
                        path.LineTo(0, height);
                        path.Close();
                    }
                    else
                    {
                        path.MoveTo(0, 0);
                        path.LineTo(width - arcHeightAbs, 0);
                        path.QuadTo(width + arcHeightAbs, height / 2f, width - arcHeightAbs, height);
                        path.LineTo(0, height);
                        path.Close();
                    }
                    break;
                case ShapePosition.Bottom:
                    if (shape.IsCropInside)
                    {
                        path.MoveTo(0, 0);
                        path.LineTo(0, height);
                        path.QuadTo(width / 2f, height - 2 * arcHeightAbs, width, height);
                        path.LineTo(width, 0);
                        path.Close();
                    }
                    else
                    {
                        path.MoveTo(0, 0);
                        path.LineTo(0, height - arcHeightAbs);
                        path.QuadTo(width / 2f, height + arcHeightAbs, width, height - arcHeightAbs);
                        path.LineTo(width, 0);
                        path.Close();
                    }
                    break;
            }
        }
    }
}
