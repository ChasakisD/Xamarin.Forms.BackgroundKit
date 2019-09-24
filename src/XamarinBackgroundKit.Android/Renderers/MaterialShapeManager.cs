using System;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.View;
using XamarinBackgroundKit.Android.Extensions;
using XamarinBackgroundKit.Android.OutlineProviders;
using XamarinBackgroundKit.Android.PathProviders;
using XamarinBackgroundKit.Shapes;
using AView = Android.Views.View;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class MaterialShapeManager : IDisposable
    {
        private bool _disposed;
        private Path _clipPath;
        private Path _maskPath;
        private Paint _maskPaint;
        private AView _nativeView;
        private IBackgroundShape _shape;
        private IPathProvider _pathProvider;

        public MaterialShapeManager()
        {
            _clipPath = new Path();
            _maskPath = new Path();
            _maskPaint = new Paint(PaintFlags.AntiAlias);
            _maskPaint.SetXfermode(BackgroundKit.PorterDuffClearMode);
        }

        #region Element Setters

        public void SetShape(AView view, IBackgroundShape newShape)
        {
            _nativeView = view;

            if (_shape != newShape)
            {
                if (_shape != null)
                {
                    _shape.ShapeInvalidateRequested -= OnShapeInvalidateRequested;
                }

                if (newShape != null)
                {
                    newShape.ShapeInvalidateRequested += OnShapeInvalidateRequested;
                }

                _shape = newShape;

                _pathProvider?.Dispose();
                _pathProvider = PathProvidersContainer.Resolve(_shape.GetType());

                if (_pathProvider != null)
                {
                    _pathProvider.SetShape(newShape);
                    _nativeView?.GetGradientDrawable()?.SetPathProvider(_pathProvider);
                }
            }

            Invalidate();
        }

        #endregion

        #region Property Changed

        private void OnShapeInvalidateRequested(object sender, EventArgs e) => Invalidate();

        #endregion

        public void Invalidate()
        {
            _pathProvider?.Invalidate();

            if (_nativeView != null)
            {
                _nativeView.OutlineProvider?.Dispose();
                _nativeView.OutlineProvider = new PathOutlineProvider(_pathProvider);
                _nativeView.ClipToOutline = true;
                _nativeView.PostInvalidate();
                _nativeView.GetGradientDrawable()?.InvalidatePath();
                _nativeView.GetGradientDrawable()?.InvalidateSelf();
            }
        }

        public void Draw(AView view, Canvas canvas, Action dispatchDraw)
        {
            if (_pathProvider == null) return;

            InitializeClipPath(canvas.Width, canvas.Height);

            var saveCount = canvas.SaveLayer(0, 0, canvas.Width, canvas.Height, null);
            dispatchDraw();
            canvas.DrawPath(_maskPath, _maskPaint);
            canvas.RestoreToCount(saveCount);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop && ViewCompat.GetElevation(view) > 0)
            {
                view.OutlineProvider = _clipPath.IsConvex ? new PathOutlineProvider(_pathProvider) : null;
            }
        }

        private void InitializeClipPath(int width, int height)
        {
            if (width <= 0 || height <= 0) return;

            var canvasBounds = new RectF(0, 0, width, height);
            canvasBounds.Inset(-1, -1);

            /* Always prefer border path. If there is no need, the provider will return the default one */
            var clipPath = _pathProvider.CreateBorderedPath(width, height);

            _clipPath.Reset();
            _clipPath.Set(clipPath);

            _maskPath.Reset();
            _maskPath.AddRect(canvasBounds, Path.Direction.Cw);
            _maskPath.InvokeOp(_clipPath, Path.Op.Difference);
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
                SetShape(null, null);

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
