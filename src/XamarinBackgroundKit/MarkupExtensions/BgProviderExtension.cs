using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.MarkupExtensions
{
    public class BgProviderExtension : IMarkupExtension<Background>
    {
        #region Background Properties

        public Color Color { get; set; }
        public bool IsRippleEnabled { get; set; }
        public Color RippleColor { get; set; }
        public bool IsClippedToBounds { get; set; } = true;

        public CornerRadius CornerRadius { get; set; }

        public double Elevation { get; set; }
        public double TranslationZ { get; set; }

        public LinearGradientBrush GradientBrush { get; set; }

        public Color BorderColor { get; set; }
        public double BorderWidth { get; set; }
        public double DashGap { get; set; }
        public double DashWidth { get; set; }
        public LinearGradientBrush BorderGradientBrush { get; set; }

        #endregion

        public Background ProvideValue(IServiceProvider serviceProvider) =>
            new Background
            {
                Color = Color,
                IsRippleEnabled = IsRippleEnabled,
                RippleColor = RippleColor,
                IsClippedToBounds = IsClippedToBounds,
                CornerRadius = CornerRadius,
                Elevation = Elevation,
                TranslationZ = TranslationZ,
                GradientBrush = GradientBrush,
                BorderColor = BorderColor,
                BorderWidth = BorderWidth,
                DashGap = DashGap,
                DashWidth = DashWidth,
                BorderGradientBrush = BorderGradientBrush
            };

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}
