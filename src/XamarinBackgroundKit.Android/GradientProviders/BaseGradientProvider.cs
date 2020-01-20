using Android.Graphics;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.Android.GradientProviders
{
    public abstract class BaseGradientProvider<T> : IGradientProvider where T : GradientBrush
    {
        private bool _disposed;

        public abstract bool HasGradient { get; protected set; }

        void IGradientProvider.SetGradient(GradientBrush gradientBrush) =>
            OnGradientSet((T)gradientBrush);

        void IGradientProvider.DrawGradient(Paint paint, int width, int height) =>
            DrawGradient(paint, width, height);

        public abstract void OnGradientSet(T gradientBrush);
        public abstract void DrawGradient(Paint paint, int width, int height);
        public abstract void ClearGradient(Paint paint, int width, int height);

        public virtual void DrawOrClearGradient(Paint paint, int width, int height)
        {
            if (!HasGradient)
            {
                ClearGradient(paint, width, height);
                return;
            }

            DrawGradient(paint, width, height);
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
