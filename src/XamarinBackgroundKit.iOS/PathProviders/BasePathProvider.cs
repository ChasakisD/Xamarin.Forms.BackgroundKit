using CoreGraphics;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.iOS.PathProviders
{
    public abstract class BasePathProvider<TShape> : IPathProvider where TShape : class, IBackgroundShape
    {
        private bool _disposed;
        private IBackgroundShape _shape;

        public CGPath Path { get; set; }
        public CGPath BorderPath { get; set; }
        public bool IsPathDirty { get; private set; }
        public bool IsBorderPathDirty { get; private set; }

        public virtual bool CanHandledByOutline => false;

        public abstract bool IsBorderSupported { get; }

        protected BasePathProvider()
        {
            _shape = new Rect();

            Invalidate();
        }

        public void Invalidate()
        {
            IsPathDirty = true;
            IsBorderPathDirty = true;
        }

        public CGPath CreatePath(CGRect bounds)
        {
            /* If the path is not dirty return it */
            if (!IsPathDirty || !(_shape is TShape tShape)) return Path;

            CreatePath(tShape, bounds);

            IsPathDirty = false;

            return Path;
        }

        public CGPath CreateBorderedPath(CGRect bounds)
        {
            /* If the path provider, does not support border, use the default */
            if (!IsBorderSupported) return CreatePath(bounds);

            /* If the path is not dirty return it */
            if (!IsBorderPathDirty || !(_shape is TShape tShape)) return BorderPath;

            /* If there is no need to create border, return the normal path */
            if (!tShape.NeedsBorderInset) return CreatePath(bounds);

            CreateBorderedPath(tShape, bounds, tShape.Background.BorderWidth);

            IsBorderPathDirty = false;

            return BorderPath;
        }

        public virtual void SetShape(IBackgroundShape shape)
        {
            _shape = shape;

            Invalidate();
        }

        public abstract void CreatePath(TShape shape, CGRect bounds);

        public virtual void CreateBorderedPath(TShape shape, CGRect bounds, double strokeWidth) { }

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
