using System;
using Android.Graphics;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.Android.PathProviders
{
    public class DiagonalPathProvider : BasePathProvider<Diagonal>
    {
        public override bool IsBorderSupported => false;

        public override void CreatePath(Path path, Diagonal shape, int width, int height)
        {
            var angleAbs = Math.Abs(shape.Angle);
            var isDirLeft = shape.Direction == ShapeDirection.Left;
            var diagonalHeight = (float)(width * Math.Tan(Math.PI / 180 * angleAbs));

            switch (shape.Position)
            {
                case ShapePosition.Left:
                    if (isDirLeft)
                    {
                        path.MoveTo(diagonalHeight, 0);
                        path.LineTo(width, 0);
                        path.LineTo(width, height);
                        path.LineTo(0, height);
                    }
                    else
                    {
                        path.MoveTo(0, 0);
                        path.LineTo(width, 0);
                        path.LineTo(width, height);
                        path.LineTo(diagonalHeight, height);
                    }
                    break;
                case ShapePosition.Top:
                    if (isDirLeft)
                    {
                        path.MoveTo(width, height);
                        path.LineTo(width, diagonalHeight);
                        path.LineTo(0, 0);
                        path.LineTo(0, height);
                    }
                    else
                    {
                        path.MoveTo(width, height);
                        path.LineTo(width, 0);
                        path.LineTo(0, diagonalHeight);
                        path.LineTo(0, height);
                    }
                    break;
                case ShapePosition.Bottom:
                    if (isDirLeft)
                    {
                        path.MoveTo(0, 0);
                        path.LineTo(width, 0);
                        path.LineTo(width, height - diagonalHeight);
                        path.LineTo(0, height);
                    }
                    else
                    {
                        path.MoveTo(width, height);
                        path.LineTo(0, height - diagonalHeight);
                        path.LineTo(0, 0);
                        path.LineTo(width, 0);
                    }
                    break;
                case ShapePosition.Right:
                    if (isDirLeft)
                    {
                        path.MoveTo(0, 0);
                        path.LineTo(width, 0);
                        path.LineTo(width - diagonalHeight, height);
                        path.LineTo(0, height);
                    }
                    else
                    {
                        path.MoveTo(0, 0);
                        path.LineTo(width - diagonalHeight, 0);
                        path.LineTo(width, height);
                        path.LineTo(0, height);
                    }
                    break;
            }

            path.Close();
        }
    }
}
