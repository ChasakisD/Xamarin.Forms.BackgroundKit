using System;
using CoreAnimation;
using CoreGraphics;
using MaterialComponents;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.iOS.GradientProviders;
using XamarinBackgroundKit.iOS.PathProviders;

namespace XamarinBackgroundKit.iOS.Renderers
{
    public class GradientStrokeLayer : CALayer
    {
        private bool _dirty;
        private bool _pathDirty;

        private double _dashGap;
        private double _dashWidth;

        private CGRect _bounds;

        private UIColor _color;

        private float _strokeWidth;
        private UIColor _strokeColor;

        private CGColor _defaultShadowColor;

        private ShadowLayer _shadowLayer;

        private IPathProvider _pathProvider;
        private IGradientProvider _gradientProvider;
        private IGradientProvider _strokeGradientProvider;

        public GradientStrokeLayer() => Initialize();

        private void Initialize()
        {
            _dirty = true;
            _pathDirty = true;
            _bounds = new CGRect();
            _shadowLayer = new ShadowLayer();
            _pathProvider = new RectPathProvider();

            _defaultShadowColor = _shadowLayer.ShadowColor;

            InsertSublayer(_shadowLayer, 0);

            /*
             When creating custom layers, the contentsScale must
             be set in order to draw smooth lines. Without this line,
             context clipping is blurry and not smooth at all! 
             */
            ContentsScale = UIScreen.MainScreen.Scale;
        }

        public override void LayoutSublayers()
        {
            base.LayoutSublayers();

            if (Bounds.Equals(_bounds)) return;

            _dirty = true;
            _pathDirty = true;
            _pathProvider?.Invalidate();

            _bounds = new CGRect(Bounds.Location, Bounds.Size);
            _shadowLayer.Frame = _bounds;            
        }

        #region Actual Draw

        public override void DrawInContext(CGContext ctx)
        {
            base.DrawInContext(ctx);

            _shadowLayer.ShadowPath = GetClipPath();

            ctx.AddPath(GetClipPath());
            ctx.Clip();
            
            DrawGradient(ctx);
            DrawBorder(ctx);

            _dirty = false;
        }

        private void DrawGradient(CGContext ctx)
        {
            if (_gradientProvider != null && _gradientProvider.HasGradient)
            {
                _gradientProvider.DrawGradient(ctx, Bounds);
            }
            else if (_color != null)
            {
                ctx.SetFillColor(_color.CGColor);
                ctx.AddPath(GetClipPath());
                ctx.DrawPath(CGPathDrawingMode.Fill);
            }
        }
        
        private void DrawBorder(CGContext ctx)
        {
            if (!HasBorder()) return;
            
            if (IsBorderDashed())
            {
                ctx.SetLineDash(0, new [] {(nfloat)_dashWidth, (nfloat)_dashGap});
            }

            // Stroke is inner, the outer will be clipped. So double the value to get the real one!
            ctx.SetLineWidth(2 * _strokeWidth);
            ctx.AddPath(GetClipPath());

            if (_strokeGradientProvider != null && _strokeGradientProvider.HasGradient)
            {
                ctx.ReplacePathWithStrokedPath();
                ctx.Clip();

                _strokeGradientProvider.DrawGradient(ctx, Bounds);
            }
            else if(_strokeColor != null)
            {
                ctx.SetStrokeColor(_strokeColor.CGColor);
                ctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        #endregion

        #region Draw Helpers

        private bool IsBorderDashed()
        {
            return _dashGap > 0 && _dashWidth > 0;
        }

        private bool HasBorder()
        {
            return _strokeWidth > 0;
        }

        public CGPath GetClipPath()
        {
            if (_pathProvider == null) return new CGPath();

            if (_pathDirty || _pathProvider.IsPathDirty)
            {
                _pathDirty = false;
                _pathProvider.CreatePath(Bounds);
            }

            return new CGPath(_pathProvider.Path);
        }

        #endregion
        
        #region Public Setters

        public void InvalidatePath()
        {
            _pathDirty = true;
        }

        public void SetPathProvider(IPathProvider pathProvider)
        {
            _pathDirty = true;
            _pathProvider = pathProvider;

            SetNeedsDisplay();
        }

        public void SetColor(Color color)
        {
            _dirty = true;
            _color = color.ToUIColor();
            
            SetNeedsDisplay();
        }
        
        public void SetElevation(double elevation)
        {
            _shadowLayer.Elevation = (float) elevation;            
        }

        public void SetShadowColor(Color color)
        {
            _shadowLayer.ShadowColor = color == Color.Default
                ? _defaultShadowColor
                : color.ToCGColor();
        }
        
        public void SetDashedBorder(double dashWidth, double dashGap)
        {
            _dirty = true;
            _dashGap = dashGap;
            _dashWidth = dashWidth;
            
            SetNeedsDisplay();
        }
        
        public void SetGradient(GradientBrush gradientBrush)
        {
            _dirty = true;

            _gradientProvider?.Dispose();
            _gradientProvider = GradientProvidersContainer.Resolve(
                gradientBrush.GetType());
            _gradientProvider?.SetGradient(gradientBrush);
            
            SetNeedsDisplay();
        }

        public void SetBorder(double strokeWidth, Color strokeColor, GradientBrush gradientBrush)
        {
            _dirty = true;
            _strokeWidth = (float) strokeWidth;
            _strokeColor = strokeColor.ToUIColor();

            _strokeGradientProvider?.Dispose();
            _strokeGradientProvider = GradientProvidersContainer.Resolve(
                gradientBrush.GetType());
            _strokeGradientProvider?.SetGradient(gradientBrush);

            SetNeedsDisplay();
        }

        #endregion

        #region LifeCycle

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_pathProvider != null)
                {
                    _pathProvider.Dispose();
                    _pathProvider = null;
                }

                _shadowLayer.RemoveFromSuperLayer();
                _shadowLayer.Dispose();
                _shadowLayer = null;

                if (_gradientProvider != null)
                {
                    _gradientProvider.Dispose();
                    _gradientProvider = null;
                }

                if (_strokeGradientProvider != null)
                {
                    _strokeGradientProvider.Dispose();
                    _strokeGradientProvider = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}