using System;
using Xamarin.Forms.Xaml;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.MarkupExtensions
{
    public class BgProviderExtension : Background, IMarkupExtension<Background>
    {
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
