using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.MarkupExtensions
{
    [ContentProperty(nameof(Gradients))]
    public class LinearGradientExtension : IMarkupExtension<LinearGradientBrush>
    {
        public Color Start { get; set; } = Color.Default;
        public Color End { get; set; } = Color.Default;
        public float Angle { get; set; }
        public string Gradients { get; set; }

        public LinearGradientBrush ProvideValue(IServiceProvider serviceProvider)
        {
            if (Start != Color.Default && End != Color.Default) return FromStartEnd();
            return !string.IsNullOrWhiteSpace(Gradients) ? FromString() : null;
        }

        private LinearGradientBrush FromStartEnd()
        {
            return new LinearGradientBrush
            {
                Angle = Angle,
                Gradients =
                {
                    new GradientStop(Start, 0),
                    new GradientStop(End, 1)
                }
            };
        }

        private LinearGradientBrush FromString()
        {
            var gradients = new List<GradientStop>();
            var gradientStrings = Gradients.Split(',');
            if (gradientStrings.Length < 2) return null;

            var offset = 0f;
            var offsetDelta = 1f / (gradientStrings.Length - 1);
            foreach (var gradientString in gradientStrings)
            {
                var trimmedGradientString = gradientString.Trim();

                Color? color = null;
                try
                {
                    if (trimmedGradientString.Contains("#"))
                    {
                        color = Color.FromHex(trimmedGradientString);
                    }
                    else
                    {
                        var systemColor = System.Drawing.Color.FromName(trimmedGradientString);
                        color = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                if (color == null) return null;
                gradients.Add(new GradientStop(color.Value, offset));

                offset += offsetDelta;
            }

            return new LinearGradientBrush
            {
                Angle = Angle,
                Gradients = gradients
            };

        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}
