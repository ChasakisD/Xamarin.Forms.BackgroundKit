using System;
using Android.Graphics;
using XamarinBackgroundKit.Shapes;
using Rect = XamarinBackgroundKit.Shapes.Rect;

namespace XamarinBackgroundKit.Android.PathProviders
{
    public abstract class BasePathProvider<TShape> : IPathProvider where TShape : class, IBackgroundShape
    {
        private bool _disposed;
        private IBackgroundShape _shape;

        public Path Path { get; private set; }
        public Path BorderPath { get; private set; }
        public bool IsPathDirty { get; private set; }
        public bool IsBorderPathDirty { get; private set; }

        public virtual bool CanHandledByOutline => false;

        public abstract bool IsBorderSupported { get; }

        protected BasePathProvider()
        {
            _shape = new Rect();

            Path = new Path();
            BorderPath = new Path();

            Invalidate();
        }

        public void Invalidate()
        {
            IsPathDirty = true;
            IsBorderPathDirty = true;
        }

        public Path CreatePath(int width, int height)
        {
            if (_disposed || Path.Handle == IntPtr.Zero) return null;

            /* If the path is not dirty return it */
            if (!IsPathDirty || !(_shape is TShape tShape)) return Path;

            Path.Reset();

            CreatePath(Path, tShape, width, height);

            IsPathDirty = false;

            return Path;
        }

        public Path CreateBorderedPath(int width, int height)
        {
            if (_disposed || Path.Handle == IntPtr.Zero) return null;

            /* If the path provider, does not support border, use the default */
            if (!IsBorderSupported) return CreatePath(width, height);

            /* If the path is not dirty return it */
            if (!IsBorderPathDirty || !(_shape is TShape tShape)) return BorderPath;

            /* If there is no need to create border, return the normal path */
            if (!tShape.NeedsBorderInset) return CreatePath(width, height);

            var strokeWidth = (int)(tShape.BorderWidth * BackgroundKit.Density);

            BorderPath.Reset();

            CreateBorderedPath(BorderPath, tShape, width, height, strokeWidth);

            IsBorderPathDirty = false;

            return BorderPath;
        }

        public virtual void SetShape(IBackgroundShape shape)
        {
            if (_disposed) return;

            _shape = shape;

            Invalidate();
        }

        public abstract void CreatePath(Path path, TShape shape, int width, int height);

        public virtual void CreateBorderedPath(Path path, TShape shape, int width, int height, int strokeWidth) { }

        public void Dispose() => Dispose(true);

        public virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;

            _shape = null;

            if (Path != null)
            {
                Path.Dispose();
                Path = null;
            }

            if (BorderPath != null)
            {
                BorderPath.Dispose();
                BorderPath = null;
            }
        }
    }
}
