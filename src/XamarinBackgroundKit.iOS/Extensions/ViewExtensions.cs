using CoreAnimation;
using MaterialComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.iOS.Renderers;
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

        public static void SetMaterialElevation(this UIView view, IElevationElement elevationElement)
        {
            view.SetMaterialElevation(elevationElement.Elevation);
        }

        public static void SetMaterialElevation(this UIView view, double elevation)
        {
            view.FindLayerOfType<GradientStrokeLayer>()?.SetElevation(elevation);
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
            view.SetBorder(borderElement.BorderColor, borderElement.BorderWidth, borderElement.BorderGradients,
                borderElement.BorderAngle);
            view.SetDashedBorder(borderElement.DashWidth, borderElement.DashGap);
        }

        private static void SetBorder(this UIView view, Color color, double width, IList<GradientStop> gradients, float angle)
        {
            view.FindLayerOfType<GradientStrokeLayer>()?.SetBorder(width, color, gradients, angle);
        }

        private static void SetDashedBorder(this UIView view, double dashWidth, double dashGap)
        {
            view.FindLayerOfType<GradientStrokeLayer>()?.SetDashedBorder(dashWidth, dashGap);
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
            view.FindLayerOfType<GradientStrokeLayer>()?.SetCornerRadius(cornerRadius);
        }

        #endregion

        #region Gradients

        public static void SetGradient(this UIView view, IGradientElement gradientElement)
        {
            view.SetGradient(gradientElement.GradientType, gradientElement.Gradients, gradientElement.Angle);
        }

        public static void SetGradient(this UIView view, GradientType type, IList<GradientStop> gradients, float angle)
        {
            view.FindLayerOfType<GradientStrokeLayer>()?.SetGradient(gradients, angle);
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
            if (subLayer == null)
            {
                subLayer = new GradientStrokeLayer();
                view.Layer?.InsertSublayer(subLayer, 0);
            }

            return (T) subLayer;
        }
    }
}