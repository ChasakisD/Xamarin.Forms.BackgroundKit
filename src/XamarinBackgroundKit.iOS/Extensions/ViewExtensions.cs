using CoreAnimation;
using CoreGraphics;
using Foundation;
using MaterialComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;
using IBorderElement = XamarinBackgroundKit.Abstractions.IBorderElement;
using MCard = MaterialComponents.Card;

namespace XamarinBackgroundKit.iOS.Extensions
{
    public static class ViewExtensions
    {
        #region Elevation

        public static void SetElevation(this MCard view, IElevationElement elevationElement)
        {
            view.SetShadowElevation((nfloat)elevationElement.Elevation, UIControlState.Normal);
        }

        public static void SetElevation(this ChipView view, IElevationElement elevationElement)
        {
            view.SetElevation((nfloat)elevationElement.Elevation, UIControlState.Normal);
        }

        public static void SetElevation(this UIView view, IElevationElement elevationElement)
        {
            view.SetElevation(elevationElement.Elevation);
        }

        public static void SetElevation(this UIView view, double elevation)
        {
            if (view?.Layer == null || elevation <= 0) return;

            view.Layer.MasksToBounds = false;
            view.Layer.ShadowOpacity = 0.24f;
            view.Layer.ShadowRadius = (float)elevation;
            view.Layer.ShadowColor = Color.Black.ToCGColor();
            view.Layer.ShadowOffset = new CGSize(0, elevation);
        }

        public static void SetMaterialElevation(this UIView view, IElevationElement elevationElement)
        {
            view.SetMaterialElevation(elevationElement.Elevation);
        }

        public static void SetMaterialElevation(this UIView view, double elevation)
        {
            var shadowLayer = view.FindLayerOfType<ShadowLayer>();
            if (shadowLayer == null)
            {
                shadowLayer = new ShadowLayer();

                view.Layer.InsertSublayer(shadowLayer, 0);
            }

            if(!ReferenceEquals(view.Layer, shadowLayer))
            {
                shadowLayer.Frame = view.Bounds;
            }

            shadowLayer.Elevation = (float) elevation;
            
            CALayer currentLayer = view.FindLayerOfType<CAGradientLayer>();
            if (currentLayer == null)
            {
                currentLayer = view.Layer;
                if (currentLayer == null) return;
            }

            if (currentLayer.CornerRadius > 0)
            {
                shadowLayer.CornerRadius = currentLayer.CornerRadius;
            }
        }

        #endregion

        #region Border

        public static void SetBorder(this ChipView view, IBorderElement borderElement)
        {
            view.SetBorder(borderElement.BorderColor, borderElement.BorderWidth);
        }

        public static void SetBorder(this ChipView view, Color color, double width)
        {
            view.SetBorderWidth((nfloat)width, UIControlState.Normal);
            view.SetBorderColor(color.ToUIColor(), UIControlState.Normal);
        }

        public static void SetBorder(this MCard view, IBorderElement borderElement)
        {
            view.SetBorder(borderElement.BorderColor, borderElement.BorderWidth);
        }

        public static void SetBorder(this MCard view, Color color, double width)
        {
            if (color == Color.Default)
            {
                view.SetBorderColor(UIColor.Clear, UIControlState.Normal);
            }
            else
            {
                view.SetBorderWidth((nfloat)width, UIControlState.Normal);
                view.SetBorderColor(color.ToUIColor(), UIControlState.Normal);
            }
        }

        public static void SetBorder(this UIView view, IBorderElement borderElement)
        {
            view.SetBorder(borderElement.BorderColor, borderElement.BorderWidth);
        }

        public static void SetBorder(this UIView view, Color color, double width)
        {
            CALayer currentLayer = view.FindLayerOfType<CAGradientLayer>();
            if (currentLayer == null)
            {
                currentLayer = view.Layer;
                if (currentLayer == null) return;
            }

            if (!ReferenceEquals(view.Layer, currentLayer))
            {
                currentLayer.Frame = view.Bounds;
            }

            currentLayer.BorderColor = color.ToCGColor();
            currentLayer.BorderWidth = (nfloat)width;
        }

        #endregion

        #region Corner Radius

        public static void SetCornerRadius(this MCard view, ICornerElement cornerElement)
        {
            view.CornerRadius = (nfloat)cornerElement.CornerRadius.TopLeft;
        }

        public static void SetCornerRadius(this UIView view, ICornerElement cornerElement)
        {
            view.SetCornerRadius(cornerElement.CornerRadius);
        }

        public static void SetCornerRadius(this UIView view, CornerRadius cornerRadius)
        {
            if (cornerRadius.IsEmpty()) return;

            CALayer currentLayer = view.FindLayerOfType<CAGradientLayer>();
            if (currentLayer == null)
            {
                currentLayer = view.Layer;
            }

            if (currentLayer == null) return;

            /* 
             * Calculate the avg radius, and compare it with one. 
             * If is the same with one then all corners have the same radius
            */
            if (cornerRadius.IsAllRadius())
            {
                currentLayer.CornerRadius = (float)cornerRadius.TopLeft;
                return;
            }

            var topLeft = (float)cornerRadius.TopLeft;
            var topRight = (float)cornerRadius.TopRight;
            var bottomLeft = (float)cornerRadius.BottomLeft;
            var bottomRight = (float)cornerRadius.BottomRight;

            var bounds = view.Bounds;
            var bezierPath = new UIBezierPath();
            bezierPath.AddArc(new CGPoint((float)bounds.X + bounds.Width - topRight, (float)bounds.Y + topRight), topRight, (float)(Math.PI * 1.5), (float)Math.PI * 2, true);
            bezierPath.AddArc(new CGPoint((float)bounds.X + bounds.Width - bottomRight, (float)bounds.Y + bounds.Height - bottomRight), bottomRight, 0, (float)(Math.PI * .5), true);
            bezierPath.AddArc(new CGPoint((float)bounds.X + bottomLeft, (float)bounds.Y + bounds.Height - bottomLeft), bottomLeft, (float)(Math.PI * .5), (float)Math.PI, true);
            bezierPath.AddArc(new CGPoint((float)bounds.X + topLeft, (float)bounds.Y + topLeft), topLeft, (float)Math.PI, (float)(Math.PI * 1.5), true);

            currentLayer.Mask?.Dispose();
            currentLayer.Mask = new CAShapeLayer { Frame = view.Bounds, Path = bezierPath.CGPath };
        }

        #endregion

        #region Gradients

        public static void SetGradient(this UIView view, VisualElement element)
        {
            if (!(element is IMaterialVisualElement supportElement)) return;

            view.SetGradient(supportElement.GradientType, supportElement.Gradients, supportElement.Angle, view.Bounds);
        }

        public static void SetGradient(this UIView view, IGradientElement gradientElement)
        {
            view.SetGradient(gradientElement.GradientType, gradientElement.Gradients, gradientElement.Angle, view.Bounds);
        }

        public static void SetGradient(this UIView view, IGradientElement gradientElement, IList<GradientStop> oldGradients)
        {
            view.SetGradient(gradientElement.GradientType, gradientElement.Gradients, gradientElement.Angle, view.Bounds, oldGradients);
        }

        public static void SetGradient(this UIView view, IGradientElement gradientElement, CGRect rect)
        {
            view.SetGradient(gradientElement.GradientType, gradientElement.Gradients, gradientElement.Angle, rect);
        }

        public static void SetGradient(this UIView view, IGradientElement gradientElement, CGRect rect, IList<GradientStop> oldGradients)
        {
            view.SetGradient(gradientElement.GradientType, gradientElement.Gradients, gradientElement.Angle, rect, oldGradients);
        }

        public static void SetGradient(this UIView view, GradientType type, IList<GradientStop> gradients, float angle)
        {
            view.SetGradient(type, gradients, angle, view.Bounds);
        }

        public static void SetGradient(
            this UIView view,
            GradientType type,
            IList<GradientStop> gradients,
            float angle,
            CGRect rect,
            IList<GradientStop> oldGradients = null)
        {
            if (view.Layer == null || !gradients.Any()) return;

            if (gradients.AreEqual(oldGradients)) return;

            var positions = angle.ToStartEndPoint();

            for (var i = 0; i < positions.Length; i++)
            {
                if (!(positions[i] > 1)) continue;
                positions[i] = 1;
            }

            var colors = gradients.Select(x => x.Color.ToCGColor()).ToArray();
            var colorPositions = gradients.Select(x => new NSNumber(x.Offset)).ToArray();
            
            var currentLayer = view.FindLayerOfType<CAGradientLayer>();
            if (currentLayer == null)
            {
                currentLayer = new CAGradientLayer();

                view.Layer.InsertSublayer(currentLayer, 0);
            }

            if(!view.Layer.Equals(currentLayer))
            {
                currentLayer.Frame = rect;
            }
            
            currentLayer.Colors = colors;
            currentLayer.Locations = colorPositions;
            currentLayer.StartPoint = new CGPoint(positions[0], positions[1]);
            currentLayer.EndPoint = new CGPoint(positions[2], positions[3]);
        }

        #endregion

        #region Platform Rendering

        public static IVisualElementRenderer GetOrCreateRenderer(this View view) =>
             GetRenderer(view);

        public static IVisualElementRenderer GetRenderer(this View view)
        {
            /* Create the Native Renderer if not initialized */
            if (Platform.GetRenderer(view) == null)
            {
                var ctxRenderer = Platform.CreateRenderer(view);
                Platform.SetRenderer(view, ctxRenderer);
            }

            /* Render the X.F. View */
            return Platform.GetRenderer(view);
        }

        #endregion

        public static T FindLayerOfType<T>(this UIView view) where T : CALayer
        {
            if (view.Layer is T layer) return layer;

            var subLayer = view.Layer?.Sublayers?.FirstOrDefault(x => x is T);

            return (T) subLayer;
        }
    }
}