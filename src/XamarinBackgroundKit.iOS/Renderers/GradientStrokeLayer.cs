using CoreAnimation;
using CoreGraphics;
using MaterialComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.iOS.Renderers
{
    public class GradientStrokeLayer : CALayer
    {
        private double _dashGap;
        private double _dashWidth;
        private CornerRadius _cornerRadius;

        private UIColor _color;
        private CGColor[] _colors;
        private float[] _positions;
        private nfloat[] _colorPositions;

        private float _strokeWidth;
        private UIColor _strokeColor;
        private CGColor[] _strokeColors;
        private float[] _strokePositions;
        private nfloat[] _strokeColorPositions;

        private ShadowLayer _shadowLayer;

        public GradientStrokeLayer() => Initialize();

        private void Initialize()
        {
            _shadowLayer = new ShadowLayer();

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
            
            _shadowLayer.Frame = Bounds;
            _shadowLayer.ShadowPath = GetRoundCornersPath(Bounds).CGPath; 
        }

        #region Actual Draw

        public override void DrawInContext(CGContext ctx)
        {
            base.DrawInContext(ctx);

            ctx.AddPath(GetRoundCornersPath(Bounds).CGPath);
            ctx.Clip();
            
            DrawGradient(ctx);
            DrawBorder(ctx);
        }

        private void DrawGradient(CGContext ctx)
        {
            if (HasGradient())
            {
                var startPoint = new CGPoint(_positions[0] * Bounds.Width, _positions[1] * Bounds.Height);
                var endPoint = new CGPoint(_positions[2] * Bounds.Width, _positions[3] * Bounds.Height);

                var gradient = new CGGradient(CGColorSpace.CreateDeviceRGB(), _colors, _colorPositions);
            
                ctx.DrawLinearGradient(gradient, startPoint, endPoint, CGGradientDrawingOptions.None);
            }
            else if(_color != null)
            {
                ctx.SetFillColor(_color.CGColor);
                ctx.AddPath(GetRoundCornersPath(Bounds).CGPath);
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
            ctx.AddPath(GetRoundCornersPath(Bounds).CGPath);

            if (HasBorderGradient())
            {
                ctx.ReplacePathWithStrokedPath();
                ctx.Clip();
                
                var startPoint = new CGPoint(_strokePositions[0] * Bounds.Width, _strokePositions[1] * Bounds.Height);
                var endPoint = new CGPoint(_strokePositions[2] * Bounds.Width, _strokePositions[3] * Bounds.Height);

                var gradient = new CGGradient(CGColorSpace.CreateDeviceRGB(), _strokeColors, _strokeColorPositions);
                
                ctx.DrawLinearGradient(gradient, startPoint, endPoint, CGGradientDrawingOptions.None);
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
        
        private bool HasGradient()
        {
            return _colors != null && _positions != null && _colorPositions != null;
        }
        
        private bool HasBorderGradient()
        {
            return _strokeColors != null && _strokePositions != null && _strokeColorPositions != null;
        }

        public UIBezierPath GetRoundCornersPath() => GetRoundCornersPath(Bounds);

        public UIBezierPath GetRoundCornersPath(CGRect bounds) =>
            BackgroundKit.GetRoundCornersPath(bounds, _cornerRadius);

        #endregion
        
        #region Public Setters
        
        public void SetColor(Color color)
        {
            _color = color.ToUIColor();
            
            SetNeedsDisplay();
        }
        
        public void SetElevation(double elevation)
        {
            _shadowLayer.Elevation = (float) elevation;            
        }

        public void SetShadowColor(CGColor color)
        {
            _shadowLayer.ShadowColor = color;
        }

        public void SetCornerRadius(CornerRadius cornerRadius)
        {
            _cornerRadius = cornerRadius;
            
            _shadowLayer.ShadowPath = GetRoundCornersPath(Bounds).CGPath; 
            
            SetNeedsDisplay();
        }
        
        public void SetDashedBorder(double dashWidth, double dashGap)
        {
            _dashGap = dashGap;
            _dashWidth = dashWidth;
            
            SetNeedsDisplay();
        }
        
        public void SetGradient(IList<GradientStop> gradients, float angle)
        {
            if (gradients == null || gradients.Count < 2)
            {
                _colors = null;
                _positions = null;
                _colorPositions = null;
            }
            else
            {
                _positions = angle.ToStartEndPoint();

                for (var i = 0; i < _positions.Length; i++)
                {
                    if (!(_positions[i] > 1)) continue;
                    _positions[i] = 1;
                }

                _colors = gradients.Select(x => x.Color.ToCGColor()).ToArray();
                _colorPositions = gradients.Select(x => (nfloat) x.Offset).ToArray();
            }
            
            SetNeedsDisplay();
        }

        public void SetBorder(double strokeWidth, Color strokeColor, IList<GradientStop> gradients, float angle)
        {
            _strokeWidth = (float) strokeWidth;
            _strokeColor = strokeColor.ToUIColor();
            
            if (gradients == null || gradients.Count < 2)
            {
                _strokeColors = null;
                _strokePositions = null;
                _strokeColorPositions = null;
            }
            else
            {
                _strokePositions = angle.ToStartEndPoint();

                for (var i = 0; i < _strokePositions.Length; i++)
                {
                    if (!(_strokePositions[i] > 1)) continue;
                    _strokePositions[i] = 1;
                }

                _strokeColors = gradients.Select(x => x.Color.ToCGColor()).ToArray();
                _strokeColorPositions = gradients.Select(x => (nfloat) x.Offset).ToArray();
            }
            
            SetNeedsDisplay();
        }

        #endregion

        #region LifeCycle

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _shadowLayer.RemoveFromSuperLayer();
                _shadowLayer.Dispose();
                _shadowLayer = null;

                _colors = null;
                _positions = null;
                _colorPositions = null;

                _strokeColors = null;
                _strokePositions = null;
                _strokeColorPositions = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}