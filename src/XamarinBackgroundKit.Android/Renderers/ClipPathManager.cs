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
        private Path _maskPath;
        private Paint _maskPaint;

        public ClipPathManager()
        {
            _clipPath = new Path();
            _maskPath = new Path();
            _maskPaint = new Paint(PaintFlags.AntiAlias);
            _maskPaint.SetXfermode(BackgroundKit.PorterDuffClearMode);
        }

        public bool ClipCanvas(Context context, Canvas canvas, Background background, Func<bool> drawChild)
        {
            if (context == null || canvas == null || background == null || drawChild == null) return false;

            /* If the corner radius is uniform the clip is done by the ViewOutlineProvider */
            if ((background.CornerRadius.IsEmpty() || background.CornerRadius.IsAllRadius())
                && background.BorderStyle != BorderStyle.Outer) return drawChild();

            var cornerRadii = background.CornerRadius.ToRadii(context.Resources.DisplayMetrics.Density);            

            var bounds = new RectF(0, 0, canvas.Width, canvas.Height);

            bounds.Inset(-1, -1);

            switch (background.BorderStyle)
            {
                case BorderStyle.Inner:
                    for (var i = 0; i < cornerRadii.Length; i++)
                        cornerRadii[i] -= cornerRadii[i] <= 0 ? 0 : 1;

                    _clipPath.Reset();
                    _clipPath.AddRoundRect(bounds, cornerRadii, Path.Direction.Cw);
                    break;
                case BorderStyle.Outer:
                    var strokeWidth = (int)context.ToPixels(background.BorderWidth);

                    for (var i = 0; i < cornerRadii.Length; i++)
                        cornerRadii[i] -= cornerRadii[i] <= 0 ? 0 : strokeWidth;

                    bounds.Inset(strokeWidth, strokeWidth);

                    _clipPath.Reset();
                    _clipPath.AddRoundRect(bounds, cornerRadii, Path.Direction.Cw);
                    break;
            }

            _maskPath.Reset();
            _maskPath.AddRect(0, 0, canvas.Width, canvas.Height, Path.Direction.Cw);
            _maskPath.InvokeOp(_clipPath, Path.Op.Difference);

            var saveCount = canvas.SaveLayer(0, 0, canvas.Width, canvas.Height, null);
            var drawChildResult = drawChild();
            canvas.DrawPath(_maskPath, _maskPaint);
            canvas.RestoreToCount(saveCount);

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

                if (_maskPath != null)
                {
                    _maskPath.Dispose();
                    _maskPath = null;
                }

                if (_maskPaint != null)
                {
                    _maskPaint.Dispose();
                    _maskPaint = null;
                }
            }
        }
    }
}
