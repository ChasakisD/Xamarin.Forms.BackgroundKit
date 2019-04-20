using System;
using System.Collections.Generic;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MaterialComponents;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.iOS.Renderers
{
    public class GradientStrokeLayer : CAGradientLayer
    {
        private CornerRadius _cornerRadius;
        
        private CGColor[] _colors;
        private float[] _positions;
        private NSNumber[] _colorPositions;

        private CGColor[] _strokeColors;
        private float[] _strokePositions;
        private NSNumber[] _strokeColorPositions;
        
        public CAShapeLayer MaskLayer { get; private set; }
        public ShadowLayer ShadowLayer { get; private set; }
        public CAGradientLayer GradientBorderLayer { get; private set; }

        public GradientStrokeLayer() => Initialize();

        private void Initialize()
        {
            MaskLayer = new CAShapeLayer();
            ShadowLayer = new ShadowLayer();
            GradientBorderLayer = new CAGradientLayer();
            
            MaskLayer.FillColor = UIColor.Clear.CGColor;
            MaskLayer.StrokeColor = UIColor.White.CGColor;
            
            InsertSublayer(ShadowLayer, 0);
            InsertSublayer(GradientBorderLayer, 1);
        }

        public override void LayoutSublayers()
        {
            base.LayoutSublayers();
            
            ShadowLayer.Frame = Bounds;
            GradientBorderLayer.Frame = new CGRect(CGPoint.Empty, Bounds.Size);
                
            InvalidateCornerRadius();
        }

        public void SetElevation(double elevation)
        {
            ShadowLayer.Frame = Bounds;
            ShadowLayer.Elevation = (float) elevation;
        }

        public void SetGradient(IList<GradientStop> gradients, float angle)
        {
            if (gradients == null || gradients.Count < 2)
            {
                _colors = null;
                _positions = null;
                _colorPositions = null;

                Colors = null;
                Locations = null;
                
                return;
            }
            
            _positions = angle.ToStartEndPoint();

            for (var i = 0; i < _positions.Length; i++)
            {
                if (!(_positions[i] > 1)) continue;
                _positions[i] = 1;
            }

            _colors = gradients.Select(x => x.Color.ToCGColor()).ToArray();
            _colorPositions = gradients.Select(x => new NSNumber(x.Offset)).ToArray();
            
            Colors = _colors;
            Locations = _colorPositions;
            StartPoint = new CGPoint(_positions[0], _positions[1]);
            EndPoint = new CGPoint(_positions[2], _positions[3]);
        }
        
        public void SetCornerRadius(CornerRadius cornerRadius)
        {
            _cornerRadius = cornerRadius;
            InvalidateCornerRadius();
        }
        
        public void SetDashedBorder(double dashWidth, double dashGap)
        {
            MaskLayer.LineDashPattern = dashWidth > 0 ? new NSNumber[] {dashWidth, dashGap} : null;
        }

        public void SetBorder(double strokeWidth, Color strokeColor, IList<GradientStop> gradients, float angle)
        {
            if (gradients == null || gradients.Count <= 2)
            {
                _strokeColors = new[] {strokeColor.ToCGColor(), strokeColor.ToCGColor()};
                _strokePositions = new[] {0f, 0f, 0f, 0f};
                _strokeColorPositions = new[] {new NSNumber(0), new NSNumber(1)};
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
                _strokeColorPositions = gradients.Select(x => new NSNumber(x.Offset)).ToArray();
            }
                
            MaskLayer.LineWidth = (float) strokeWidth * UIScreen.MainScreen.Scale;
                
            GradientBorderLayer.Colors = _strokeColors;
            GradientBorderLayer.Locations = _strokeColorPositions;
            GradientBorderLayer.StartPoint = new CGPoint(_strokePositions[0], _strokePositions[1]);
            GradientBorderLayer.EndPoint = new CGPoint(_strokePositions[2], _strokePositions[3]);
            
            InvalidateCornerRadius();
        }
        
        private void InvalidateCornerRadius()
        {
            if (_cornerRadius.IsAllRadius())
            {
                var uniformCornerRadius = (float) _cornerRadius.TopLeft;
                
                MaskLayer.Path = UIBezierPath.FromRoundedRect(
                    Bounds, (float) _cornerRadius.TopLeft).CGPath;
                
                CornerRadius = uniformCornerRadius;
                ShadowLayer.CornerRadius = uniformCornerRadius;
                GradientBorderLayer.CornerRadius = uniformCornerRadius;
            }
            else
            {
                var topLeft = (float)_cornerRadius.TopLeft;
                var topRight = (float)_cornerRadius.TopRight;
                var bottomLeft = (float)_cornerRadius.BottomLeft;
                var bottomRight = (float)_cornerRadius.BottomRight;

                var bezierPath = new UIBezierPath();
                bezierPath.AddArc(new CGPoint((float)Bounds.X + Bounds.Width - topRight, (float)Bounds.Y + topRight), topRight, (float)(Math.PI * 1.5), (float)Math.PI * 2, true);
                bezierPath.AddArc(new CGPoint((float)Bounds.X + Bounds.Width - bottomRight, (float)Bounds.Y + Bounds.Height - bottomRight), bottomRight, 0, (float)(Math.PI * .5), true);
                bezierPath.AddArc(new CGPoint((float)Bounds.X + bottomLeft, (float)Bounds.Y + Bounds.Height - bottomLeft), bottomLeft, (float)(Math.PI * .5), (float)Math.PI, true);
                bezierPath.AddArc(new CGPoint((float)Bounds.X + topLeft, (float)Bounds.Y + topLeft), topLeft, (float)Math.PI, (float)(Math.PI * 1.5), true);

                MaskLayer.Path = bezierPath.CGPath;
            }
            
            Mask = MaskLayer;
            ShadowLayer.Mask = MaskLayer;
            GradientBorderLayer.Mask = MaskLayer;
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ShadowLayer.RemoveFromSuperLayer();
                ShadowLayer.Dispose();
                ShadowLayer = null;
                
                GradientBorderLayer.RemoveFromSuperLayer();
                GradientBorderLayer.Dispose();
                GradientBorderLayer = null;
                
                MaskLayer?.Dispose();
                MaskLayer = null;

                _colors = null;
                _positions = null;
                _colorPositions = null;
                
                _strokeColors = null;
                _strokePositions = null;
                _strokeColorPositions = null;
            }

            base.Dispose(disposing);
        }
    }
}