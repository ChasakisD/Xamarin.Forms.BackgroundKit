using Android.Graphics;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using AndroidX.Core.View;
using System;
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

        public IPathProvider PathProvider { get; private set; }

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

                PathProvider?.Dispose();
                PathProvider = PathProvidersContainer.Resolve(_shape?.GetType());

                if (PathProvider != null)
                {
                    PathProvider.SetShape(newShape);
                    _nativeView?.GetGradientDrawable()?.SetPathProvider(PathProvider);
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
            PathProvider?.Invalidate();

            if (_nativeView != null)
            {
                _nativeView.OutlineProvider?.Dispose();
                _nativeView.OutlineProvider = new PathOutlineProvider(PathProvider);
                _nativeView.ClipToOutline = true;
                _nativeView.PostInvalidate();
                _nativeView.GetGradientDrawable()?.InvalidatePath();
                _nativeView.GetGradientDrawable()?.InvalidateSelf();

                var maskDrawable = _nativeView?.GetRippleMaskDrawable();
                if (maskDrawable != null && PathProvider != null)
                {
                    maskDrawable.Shape = new PathShape(PathProvider.Path, _nativeView.Width, _nativeView.Height);
                    maskDrawable.InvalidateSelf();
                }
            }
        }

        public void Draw(AView view, Canvas canvas, Action dispatchDraw)
        {
            if (PathProvider == null) return;

            InitializeClipPath(canvas.Width, canvas.Height);

            var saveCount = canvas.SaveLayer(0, 0, canvas.Width, canvas.Height, null);
            dispatchDraw();
            canvas.DrawPath(_maskPath, _maskPaint);
            canvas.RestoreToCount(saveCount);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop && ViewCompat.GetElevation(view) > 0)
            {
                view.OutlineProvider = _clipPath.IsConvex ? new PathOutlineProvider(PathProvider) : null;
            }
        }

        private void InitializeClipPath(int width, int height)
        {
            if (width <= 0 || height <= 0) return;

            var canvasBounds = new RectF(0, 0, width, height);
            canvasBounds.Inset(-1, -1);

            /* Always prefer border path. If there is no need, the provider will return the default one */
            var clipPath = PathProvider.CreateBorderedPath(width, height);
            if (clipPath == null) return;

            _clipPath.Reset();
            _clipPath.Set(clipPath);

            _maskPath.Reset();
            _maskPath.AddRect(canvasBounds, Path.Direction.Cw!);
            _maskPath.InvokeOp(_clipPath, Path.Op.Difference!);
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
