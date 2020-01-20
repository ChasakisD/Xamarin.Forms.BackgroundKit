using CoreGraphics;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.iOS.GradientProviders
{
    public abstract class BaseGradientProvider<T> : IGradientProvider where T : GradientBrush
    {
        private bool _disposed;

        public abstract bool HasGradient { get; protected set; }

        void IGradientProvider.SetGradient(GradientBrush gradientBrush) =>
            OnGradientSet((T)gradientBrush);

        void IGradientProvider.DrawGradient(CGContext ctx, CGRect bounds) =>
            DrawGradient(ctx, bounds);

        public abstract void OnGradientSet(T gradientBrush);
        public abstract void DrawGradient(CGContext ctx, CGRect bounds);
        public abstract void ClearGradient(CGContext ctx, CGRect bounds);

        public virtual void DrawOrClearGradient(CGContext ctx, CGRect bounds)
        {
            if (!HasGradient)
            {
                ClearGradient(ctx, bounds);
                return;
            }

            DrawGradient(ctx, bounds);
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            Dispose(_disposed);
        }

        public virtual void Dispose(bool disposing)
        {

        }
    }
}
