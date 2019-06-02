using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.MarkupExtensions
{
    public class BgProviderExtension : IMarkupExtension<Background>
    {
        public Color Color { get; set; }
        public bool IsRippleEnabled { get; set; }
        public Color RippleColor { get; set; }
        public double Elevation { get; set; }
        public double TranslationZ { get; set; }
        public CornerRadius CornerRadius { get; set; }
        public LinearGradientBrush GradientBrush { get; set; }
        
        public double DashGap { get; set; }
        public double DashWidth { get; set; }
        public Color BorderColor { get; set; }
        public double BorderWidth { get; set; }
        public BorderStyle BorderStyle { get; set; }
        public LinearGradientBrush BorderGradientBrush { get; set; }

        public BgProviderExtension()
        {
            Color = (Color)Background.ColorProperty.DefaultValue;
            RippleColor = (Color)Background.RippleColorProperty.DefaultValue;
            IsRippleEnabled = (bool)Background.IsRippleEnabledProperty.DefaultValue;

            Elevation = (double)Background.ElevationProperty.DefaultValue;
            TranslationZ = (double)Background.TranslationZProperty.DefaultValue;

            CornerRadius = (CornerRadius)Background.CornerRadiusProperty.DefaultValue;
            GradientBrush = (LinearGradientBrush)Background.GradientBrushProperty.DefaultValue;

            DashGap = (double)Background.DashGapProperty.DefaultValue;
            DashWidth = (double)Background.DashWidthProperty.DefaultValue;
            BorderColor = (Color)Background.BorderColorProperty.DefaultValue;
            BorderWidth = (double)Background.BorderWidthProperty.DefaultValue;
            BorderStyle = (BorderStyle)Background.BorderStyleProperty.DefaultValue;
            BorderGradientBrush = (LinearGradientBrush)Background.BorderGradientBrushProperty.DefaultValue;
        }

        public Background ProvideValue(IServiceProvider serviceProvider) =>
            new Background
            {
                Color = Color,
                IsRippleEnabled = IsRippleEnabled,
                RippleColor = RippleColor,
                CornerRadius = CornerRadius,
                Elevation = Elevation,
                TranslationZ = TranslationZ,
                GradientBrush = GradientBrush,
                BorderColor = BorderColor,
                BorderWidth = BorderWidth,
                BorderStyle = BorderStyle,
                DashGap = DashGap,
                DashWidth = DashWidth,
                BorderGradientBrush = BorderGradientBrush
            };

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}
