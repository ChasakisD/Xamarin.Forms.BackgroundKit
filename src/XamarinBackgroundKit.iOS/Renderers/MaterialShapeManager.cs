using System;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.iOS.Extensions;
using XamarinBackgroundKit.iOS.PathProviders;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.iOS.Renderers
{
    public class MaterialShapeManager : IDisposable
    {
        private bool _disposed;
        private IBackgroundShape _shape;
        private IPathProvider _pathProvider;
        private IVisualElementRenderer _renderer;

        #region Element Setters

        public void SetShape(IVisualElementRenderer renderer, IBackgroundShape newShape)
        {
            _renderer = renderer;

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
                    _renderer?.NativeView?.FindLayerOfType<GradientStrokeLayer>()?.SetPathProvider(_pathProvider);
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
            if (_renderer?.NativeView == null || _pathProvider == null) return;

            _pathProvider?.Invalidate();

            _renderer.NativeView.FindLayerOfType<GradientStrokeLayer>()?.InvalidatePath();
            _renderer.NativeView.FindLayerOfType<GradientStrokeLayer>()?.SetNeedsDisplay();

            if (_renderer.NativeView.Subviews == null || _renderer.Element == null) return;

            var transform = GetMaskTransform(_renderer.Element);            
            var maskPath = _pathProvider.CreateBorderedPath(_renderer.NativeView.Bounds);
            
            foreach (var subView in _renderer.NativeView.Subviews)
            {
                if (subView?.Layer == null) continue;
                
                if (subView.Layer.Sublayers?.FirstOrDefault(
                        l => l is GradientStrokeLayer) != null) continue;

                subView.Layer.Mask?.Dispose();
                subView.Layer.Mask = new CAShapeLayer
                {
                    Frame = _renderer.NativeView.Bounds,
                    Path = transform == null ? maskPath : new CGPath(maskPath, transform.Value)
                };

                subView.Layer.MasksToBounds = true;
            }
        }

        public void InvalidateInkLayer(bool isRippleEnabled)
        {
            if (_renderer?.NativeView?.Layer == null) return;

            /*
             * MDCInkView
             * From https://github.com/material-components/material-components-ios-codelabs/blob/master/MDC-111/Swift/Starter/Pods/MaterialComponents/components/Ink/src/MDCInkView.m
             * MDCInkView uses SuperView's ShadowPath in order to mask the ripple
             * So we calculate the rounded corners path and we set it to the ShadowPath
             * but with ShadowOpacity to 0 in order to not overlap the MDCShadowLayer
             */
            if (isRippleEnabled)
            {
                _renderer.NativeView.Layer.ShadowOpacity = 0;
                _renderer.NativeView.Layer.ShadowPath = _pathProvider.Path;
            }
            else
            {
                _renderer.NativeView.Layer.ShadowPath = null;
            }
        }

        private CGAffineTransform? GetMaskTransform(VisualElement visualElement)
        {
            if (!(visualElement is Layout layout)) return null;

            var minChildrenStartX = layout.Padding.Left;
            var minChildrenStartY = layout.Padding.Top;

            /*
             * To find the clipping mask for the subview we must
             * calculate the minimum start x and y for the child views.
             *
             * To find the minimum start in one axis, we must add the padding
             * of the layout and the margin of the child view.
             */
            var visualElementChildren = layout.Children?.Where(element => element is VisualElement).ToList();
            if(visualElementChildren != null && visualElementChildren.Any())
            {
                minChildrenStartX += visualElementChildren.Min(element => ((VisualElement)element).X);
                minChildrenStartY += visualElementChildren.Max(element => ((VisualElement)element).Y);
            }            

            return CGAffineTransform.MakeTranslation(-(float)minChildrenStartX, -(float)minChildrenStartY);
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
            }
        }
    }
}
