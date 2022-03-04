﻿using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;
using Google.Android.Material.Button;
using Google.Android.Material.Card;
using Google.Android.Material.Chip;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Android.Renderers;
using XamarinBackgroundKit.Controls;
using AView = Android.Views.View;
using Color = Xamarin.Forms.Color;
using IBorderElement = XamarinBackgroundKit.Abstractions.IBorderElement;
using XView = Xamarin.Forms.View;

namespace XamarinBackgroundKit.Android.Extensions
{
    public static class ViewExtensions
    {
        public static void SetColor(this AView view, Color color)
        {
            view.GetGradientDrawable()?.SetColor(color);
        }

        #region Border

        public static void SetBorder(this Chip view, Context context, IBorderElement borderElement)
        {
            view.ChipStrokeColor = ColorStateList.ValueOf(borderElement.BorderColor.ToAndroid());
            view.ChipStrokeWidth = (int)context.ToPixels(borderElement.BorderWidth);
        }

        public static void SetBorder(this MaterialButton view, Context context, IBorderElement borderElement)
        {
            view.StrokeColor = ColorStateList.ValueOf(borderElement.BorderColor.ToAndroid());
            view.StrokeWidth = (int)context.ToPixels(borderElement.BorderWidth);
        }

        public static void SetBorder(this MaterialCardView view, Context context, IBorderElement borderElement)
        {
            view.SetBorder(context, borderElement.BorderColor, borderElement.BorderWidth);
        }

        public static void SetBorder(this MaterialCardView view, Context context, Color color, double width)
        {
            if (color != Color.Default)
            {
                view.StrokeColor = color == Color.White
                    ? new global::Android.Graphics.Color(254, 254, 254)
                    : color.ToAndroid();
            }

            view.StrokeWidth = (int)context.ToPixels(width);
        }

        public static void SetBorder(this AView view, IBorderElement borderElement)
        {
            view.SetBorder(borderElement.BorderColor, borderElement.BorderWidth);
            view.SetDashedBorder(borderElement.DashWidth, borderElement.DashGap);
            view.SetBorderGradients(borderElement.BorderGradientBrush);
        }

        private static void SetBorder(this AView view, Color color, double width)
        {
            view.GetGradientDrawable()?.SetStroke(width, color);
        }

        private static void SetBorderGradients(this AView view, GradientBrush gradientBrush)
        {
            view.GetGradientDrawable()?.SetBorderGradient(gradientBrush);
        }

        private static void SetDashedBorder(this AView view, double dashWidth, double dashGap)
        {
            view.GetGradientDrawable()?.SetDashedStroke(dashWidth, dashGap);
        }

        #endregion

        #region Corner Radius

        public static void SetCornerRadius(this MaterialCardView view, Context context, ICornerElement cornerElement)
        {
            view.Radius = context.ToPixels(cornerElement.CornerRadius.TopLeft);
        }

        public static void SetCornerRadius(this MaterialButton view, Context context, ICornerElement cornerElement)
        {
            view.CornerRadius = (int)context.ToPixels(cornerElement.CornerRadius.TopLeft);
        }

        public static void SetCornerRadius(this Chip view, Context context, ICornerElement cornerElement)
        {
            view.ChipCornerRadius = context.ToPixels(cornerElement.CornerRadius.TopLeft);
        }

        #endregion

        #region Gradient

        public static void SetGradient(this AView view, IGradientElement gradientElement)
        {
            view.SetGradient(gradientElement.GradientBrush);
        }

        private static void SetGradient(this AView view, GradientBrush gradientBrush)
        {
            view.GetGradientDrawable()?.SetGradient(gradientBrush);
        }

        #endregion

        #region Elevation

        public static void SetElevation(this MaterialCardView view, Context context, IElevationElement elevationElement)
        {
            view.SetElevation(context, elevationElement.Elevation);
        }

        public static void SetElevation(this MaterialCardView view, Context context, double elevation)
        {
            view.CardElevation = context.ToPixels(elevation);
        }

        public static void SetElevation(this AView view, Context context, IElevationElement elevationElement)
        {
            view.SetElevation(context, elevationElement.Elevation);
        }

        public static void SetElevation(this AView view, Context context, double elevation)
        {
            ViewCompat.SetElevation(view, context.ToPixels(elevation));
        }

        #endregion

        #region TranslationZ

        public static void SetTranslationZ(this AView view, Context context, IElevationElement elevationElement)
        {
            view.SetTranslationZ(context, elevationElement.TranslationZ);
        }

        public static void SetTranslationZ(this AView view, Context context, double translationZ)
        {
            ViewCompat.SetTranslationZ(view, context.ToPixels(translationZ));
        }

        #endregion

        #region Rendering

        public static ViewGroup FindViewGroupParent(this AView view)
        {
            var parent = view.Parent;
            while (parent != null)
            {
                if (parent is ViewGroup viewGroup) return viewGroup;
                parent = parent.Parent;
            }

            return null;
        }

        public static IVisualElementRenderer GetOrCreateRenderer(this XView view, Context context) =>
             GetRenderer(view, context);

        public static IVisualElementRenderer GetRenderer(this XView view, Context context)
        {
            /* Create the Native Renderer if not initialized */
            if (Platform.GetRenderer(view) == null || Platform.GetRenderer(view)?.Tracker == null)
            {
                var ctxRenderer = Platform.CreateRendererWithContext(view, context);
                Platform.SetRenderer(view, ctxRenderer);
            }

            /* Render the X.F. View */
            return Platform.GetRenderer(view);
        }

        #endregion

        public static GradientStrokeDrawable GetGradientDrawable(this AView view)
        {
            switch (view.Background)
            {
                case GradientStrokeDrawable oldGradientStrokeDrawable:
                    return oldGradientStrokeDrawable;
                case RippleDrawable rippleDrawable:
                    return rippleDrawable.GetDrawable(0) as GradientStrokeDrawable;
            }

            return null;
        }

        public static ShapeDrawable GetRippleMaskDrawable(this AView view)
		{
            if (Build.VERSION.SdkInt <= BuildVersionCodes.LollipopMr1) return null;
			if (!(view.Foreground is RippleDrawable rippleDrawable)) return null;

            return rippleDrawable.GetDrawable(0) as ShapeDrawable;
		}
    }
}
