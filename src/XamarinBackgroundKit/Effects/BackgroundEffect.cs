using System.Collections.Generic;
using Xamarin.Forms;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.Effects
{
    public static class Background
    {
        #region Bindable Properties

        public static readonly BindableProperty IsOnProperty =
            BindableProperty.CreateAttached(
                "IsOn", typeof(bool), typeof(Background), false, propertyChanged: (b, o, n) =>
                b.AddOrRemoveEffect<BackgroundEffect>(() => n is bool isRounded && isRounded));

        #region Radius

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.CreateAttached(
            "CornerRadius", typeof(CornerRadius), typeof(Background), new CornerRadius(0d));

        #endregion

        #region Border

        public static readonly BindableProperty BorderColorProperty = BindableProperty.CreateAttached(
            "BorderColor", typeof(Color), typeof(Background), Color.Default);

        public static readonly BindableProperty BorderWidthProperty = BindableProperty.CreateAttached(
            "BorderWidth", typeof(double), typeof(Background), 0d);

        #endregion

        #region Gradient

        public static readonly BindableProperty AngleProperty = BindableProperty.CreateAttached(
            "Angle", typeof(float), typeof(Background), -1f);

        public static readonly BindableProperty GradientTypeProperty = BindableProperty.CreateAttached(
            "GradientType", typeof(GradientType), typeof(Background), GradientType.Linear);

        public static readonly BindableProperty GradientsProperty = BindableProperty.CreateAttached(
            "Gradients", typeof(IList<GradientStop>), typeof(Background), new List<GradientStop>(),
            defaultValueCreator: bindable => new List<GradientStop>());

        #endregion

        #endregion

        #region Getters and Setters

        public static bool GetIsOn(BindableObject view) => (bool)view.GetValue(IsOnProperty);

        public static void SetIsOn(BindableObject view, bool value) => view.SetValue(IsOnProperty, value);

        #region Radius

        public static CornerRadius GetCornerRadius(BindableObject view) => (CornerRadius)view.GetValue(CornerRadiusProperty);

        public static void SetCornerRadius(BindableObject view, CornerRadius value) => view.SetValue(CornerRadiusProperty, value);

        #endregion

        #region Border

        public static Color GetBorderColor(BindableObject view) => (Color)view.GetValue(BorderColorProperty);

        public static void SetBorderColor(BindableObject view, Color value) => view.SetValue(BorderColorProperty, value);

        public static double GetBorderWidth(BindableObject view) => (double)view.GetValue(BorderWidthProperty);

        public static void SetBorderWidth(BindableObject view, double value) => view.SetValue(BorderWidthProperty, value);

        #endregion

        #region Gradient

        public static float GetAngle(BindableObject view) => (float)view.GetValue(AngleProperty);

        public static void SetAngle(BindableObject view, float value) => view.SetValue(AngleProperty, value);

        public static GradientType GetGradientType(BindableObject view) => (GradientType)view.GetValue(GradientTypeProperty);

        public static void SetGradientType(BindableObject view, GradientType value) => view.SetValue(GradientTypeProperty, value);

        public static IList<GradientStop> GetGradients(BindableObject view) => (IList<GradientStop>)view.GetValue(GradientsProperty);

        public static void SetGradients(BindableObject view, IList<GradientStop> value) => view.SetValue(GradientsProperty, value);

        #endregion

        #endregion
    }

    public class BackgroundEffect : RoutingEffect
    {
        public BackgroundEffect() : base("XamarinBackgroundKit.BackgroundEffect") { }
    }
}