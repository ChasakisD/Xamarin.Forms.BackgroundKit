using System;
using Android.Content;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class ClipPathManager : IDisposable
    {
        private bool _disposed;

        private Path _clipPath;

        public ClipPathManager()
        {
            _clipPath = new Path();
        }

        public bool ClipCanvas(Context context, Canvas canvas, Background background, Func<bool> drawChild)
        {
            if (context == null || canvas == null || background == null || drawChild == null) return false;

            /* If the corner radius is uniform the clip is done by the ViewOutlineProvider */
            if ((background.CornerRadius.IsEmpty() || background.CornerRadius.IsAllRadius())
                && background.BorderStyle != BorderStyle.Outer) return drawChild();

            var strokeWidth = (int)context.ToPixels(background.BorderWidth);
            var cornerRadii = background.CornerRadius.ToRadii(context.Resources.DisplayMetrics.Density);

            for (var i = 0; i < cornerRadii.Length; i++)
                cornerRadii[i] -= cornerRadii[i] <= 0 ? 0 : strokeWidth;

            _clipPath.Reset();

            switch(background.BorderStyle)
            {
                case BorderStyle.Inner:
                    _clipPath.AddRoundRect(0, 0, canvas.Width, canvas.Height,
                        cornerRadii, Path.Direction.Cw);
                    break;
                case BorderStyle.Outer:
                    _clipPath.AddRoundRect(strokeWidth, strokeWidth,
                        canvas.Width - strokeWidth, canvas.Height - strokeWidth,
                        cornerRadii, Path.Direction.Cw);
                    break;
            }

            canvas.Save();
            canvas.ClipPath(_clipPath);
            var drawChildResult = drawChild();
            canvas.Restore();

            return drawChildResult;
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
                if (_clipPath != null)
                {
                    _clipPath?.Dispose();
                    _clipPath = null;
                }
            }
        }
    }
}
