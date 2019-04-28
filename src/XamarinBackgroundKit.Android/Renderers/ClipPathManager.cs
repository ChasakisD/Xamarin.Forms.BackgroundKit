using System;
using Android.Content;
using Android.Graphics;
using Xamarin.Forms;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class ClipPathManager : IDisposable
    {
        private bool _disposed;
        private bool _requiresShapeUpdate;

        private Path _roundClipPath;

        public void UpdateClipBounds()
        {
            _requiresShapeUpdate = true;
        }

        public void ClipCanvas(Context context, Canvas canvas, CornerRadius cornerRadius)
        {
            if (cornerRadius.IsEmpty()) return;

            if (_roundClipPath == null)
            {
                _roundClipPath = new Path();
            }

            if (_requiresShapeUpdate)
            {
                var cornerRadii = cornerRadius.ToRadii(context.Resources.DisplayMetrics.Density);

                _roundClipPath.Reset();
                _roundClipPath.AddRoundRect(0, 0, canvas.Width, canvas.Height, cornerRadii, Path.Direction.Cw);

                _requiresShapeUpdate = false;
            }

            canvas.ClipPath(_roundClipPath);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;

            if (_disposed) return;

            if (disposing)
            {
                if (_roundClipPath != null)
                {
                    _roundClipPath?.Dispose();
                    _roundClipPath = null;
                }
            }
        }
    }
}