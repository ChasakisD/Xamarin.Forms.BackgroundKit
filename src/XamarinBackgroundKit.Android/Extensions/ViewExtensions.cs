using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.View;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics.Drawables.Shapes;
using Android.Support.Design.Card;
using Android.Support.Design.Chip;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;
using AGradientType = Android.Graphics.Drawables.GradientType;
using AView = Android.Views.View;
using Color = Xamarin.Forms.Color;
using IBorderElement = XamarinBackgroundKit.Abstractions.IBorderElement;
using XGradientType = XamarinBackgroundKit.Controls.GradientType;
using XView = Xamarin.Forms.View;

namespace XamarinBackgroundKit.Android.Extensions
{
    public static class ViewExtensions
	{
		#region Border

		public static void SetBorder(this Chip view, Context context, IBorderElement borderElement)
		{
			if (Math.Abs(borderElement.BorderWidth) < 0.00001) return;

			view.ChipStrokeColor = ColorStateList.ValueOf(borderElement.BorderColor.ToAndroid());
			view.ChipStrokeWidth = (int)context.ToPixels(borderElement.BorderWidth);
		}

		public static void SetBorder(this MaterialCardView view, Context context, IBorderElement borderElement)
		{
			view.SetBorder(context, borderElement.BorderColor, borderElement.BorderWidth);
		}

		public static void SetBorder(this MaterialCardView view, Context context, Color color, double width)
		{
			if (Math.Abs(width) < 0.00001) return;

			view.StrokeColor = color == Color.White ? new global::Android.Graphics.Color(254, 254, 254) : color.ToAndroid();
			view.StrokeWidth = (int)context.ToPixels(width);
		}

		public static void SetBorder(this AView view, Context context, VisualElement element, IBorderElement borderElement)
		{
			view.SetBorder(context, element, borderElement.BorderColor, borderElement.BorderWidth);
		}

		public static void SetBorder(this AView view, Context context, VisualElement element, Color color, double width)
		{
			if (Math.Abs(width) < 0.00001) return;

			var borderWidth = (int)context.ToPixels(width);

			switch (view.Background)
			{
				case GradientDrawable gradientDrawable:
					gradientDrawable.SetStroke(borderWidth, ColorStateList.ValueOf(color.ToAndroid()));
					break;
                case PaintDrawable paintDrawable:
                    var paint = paintDrawable.Paint;
                    paint.AntiAlias = true;
                    paint.Color = color.ToAndroid();
                    paint.StrokeWidth = borderWidth;
                    paint.SetStyle(Paint.Style.Stroke);
                    break;
				default:
					view.Background?.Dispose();
					var newGradientDrawable = new GradientDrawable();
					newGradientDrawable.SetStroke(borderWidth, ColorStateList.ValueOf(color.ToAndroid()));
					newGradientDrawable.SetColor(element.BackgroundColor.ToAndroid());
					view.Background = newGradientDrawable;
					break;
			}
		}

		#endregion

		#region Corner Radius

		public static void SetCornerRadius(this AView view, Context context, ICornerElement cornerElement)
		{
			if (cornerElement is VisualElement element)
			{
				view.SetCornerRadius(context, element, cornerElement.CornerRadius);
			}
		}

		public static void SetCornerRadius(this MaterialCardView view, Context context, ICornerElement cornerElement)
		{
			view.Radius = context.ToPixels(cornerElement.CornerRadius.TopLeft);
		}

		public static void SetCornerRadius(this Chip view, Context context, ICornerElement cornerElement)
		{
			view.ChipCornerRadius = context.ToPixels(cornerElement.CornerRadius.TopLeft);
		}

		public static void SetCornerRadius(this AView view, Context context, VisualElement element, CornerRadius cornerRadius)
		{
			if (view == null || element == null || cornerRadius == new CornerRadius(0d)) return;

			var cornerRadii = cornerRadius.ToRadii(context.Resources.DisplayMetrics.Density);

			switch (view.Background)
			{
				case GradientDrawable gradientDrawable:
					gradientDrawable.SetCornerRadii(cornerRadii);
					break;
				case PaintDrawable paintDrawable:
					paintDrawable.SetCornerRadii(cornerRadii);
					break;
				default:
					view.Background?.Dispose();

					var newGradientDrawable = new GradientDrawable();
					newGradientDrawable.SetCornerRadii(cornerRadii);
					newGradientDrawable.SetColor(element.BackgroundColor.ToAndroid());
					view.Background = newGradientDrawable;
					break;
			}
		}

		#endregion

		#region Gradient

		public static void SetGradient(this AView view, IGradientElement gradientElement)
		{
			view.SetGradient(gradientElement.GradientType, gradientElement.Gradients, gradientElement.Angle);
		}

		public static void SetGradient(this AView view, Context context, VisualElement element)
		{
			if (!(element is IMaterialVisualElement supportElement)) return;

			view.SetGradient(supportElement.GradientType, supportElement.Gradients, supportElement.Angle);
			view.SetCornerRadius(context, supportElement);
			view.SetBorder(context, element, supportElement);
		}

		public static void SetGradient(this AView view, XGradientType type, IList<GradientStop> gradients, float angle)
		{
			if (!gradients.Any()) return;

			var colors = gradients.Select(x => (int)x.Color.ToAndroid()).ToArray();
			var positions = gradients.Select(x => x.Offset).ToArray();

            /* If positions are set, then go for PaintDrawable */
            if (positions.All(x => Math.Abs(x) < Math.Pow(10, -10)))
            {
                view.SetPaintGradient(gradients, angle);
                return;
            }

			var constructNew = true;
			GradientDrawable gradientDrawable;

			/* Reuse existing one */
			if (view.Background is GradientDrawable oldGradientDrawable)
			{
				constructNew = false;
				gradientDrawable = oldGradientDrawable;
			}
			else
			{
				gradientDrawable = new GradientDrawable();
			}
            
			gradientDrawable.SetColors(colors);
			gradientDrawable.SetOrientation(angle);

			switch (type)
			{
				case XGradientType.Radial:
					gradientDrawable.SetGradientType(AGradientType.RadialGradient);
					gradientDrawable.SetGradientRadius(view.Context.Resources.DisplayMetrics.WidthPixels);
					break;
				default:
					gradientDrawable.SetShape(ShapeType.Rectangle);
					gradientDrawable.SetGradientType(AGradientType.LinearGradient);
					break;
			}

            if (!constructNew) return;
            
            view.Background?.Dispose();
            view.Background = gradientDrawable;
        }

        private static void SetPaintGradient(this AView view, IList<GradientStop> gradients, float angle)
        {
            var constructNew = true;
            PaintDrawable paintDrawable;

            if (view.Background is PaintDrawable oldPaintDrawable)
            {
                constructNew = false;
                paintDrawable = oldPaintDrawable;
            }
            else
            {
                paintDrawable = new PaintDrawable();
            }

            paintDrawable.Shape = new RectShape();
            paintDrawable.SetShaderFactory(new GradientShaderFactory
            {
                ColorPositions = angle.ToStartEndPoint(),
                Position = gradients.Select(x => x.Offset).ToArray(),
                Colors = gradients.Select(x => (int)x.Color.ToAndroid()).ToArray()
            });

            if (!constructNew) return;

            view.Background?.Dispose();
            view.Background = paintDrawable;
        }

		#endregion

		#region Elevation

		public static void SetElevation(this MaterialCardView view, Context context, IElevationElement elevationElement)
		{
			view.SetElevation(context, elevationElement.Elevation);
		}

		public static void SetElevation(this MaterialCardView view, Context context, double elevation)
		{
			if (view == null) return;

			view.CardElevation = context.ToPixels(elevation);
		}

		public static void SetElevation(this AView view, Context context, IElevationElement elevationElement)
		{
			view.SetElevation(context, elevationElement.Elevation);
		}

		public static void SetElevation(this AView view, Context context, double elevation)
		{
			if (view == null) return;

			view.Elevation = context.ToPixels(elevation);
		}

		public static void SetTranslationZ(this AView view, float translationZ)
		{
			if (view == null) return;

			ViewCompat.SetTranslationZ(view, translationZ);
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
	}

    public class GradientShaderFactory : ShapeDrawable.ShaderFactory
    {
        public int[] Colors { private get; set; }
        public float[] Position { private get; set; }
        public float[] ColorPositions { private get; set; }

        public override Shader Resize(int width, int height)
        {
           return new LinearGradient(
                width * Position[0],
                height * Position[1],
                width * Position[2],
                height * Position[3],
                Colors,
                ColorPositions,
                Shader.TileMode.Mirror);
        }
    }
}
