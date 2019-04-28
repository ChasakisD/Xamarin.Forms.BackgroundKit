using System;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Extensions
{
    public static class CornerRadiusExtensions
    {
        private static readonly CornerRadius DefaultCornerRadius = new CornerRadius(0d);

        public static bool IsEmpty(this ICornerElement cornerRadius)
        {
            return cornerRadius.CornerRadius.IsEmpty();
        }

        public static bool IsEmpty(this CornerRadius cornerRadius)
        {
            return cornerRadius == DefaultCornerRadius;
        }

        public static bool IsAllRadius(this ICornerElement cornerRadius)
        {
            return cornerRadius.CornerRadius.IsAllRadius();
        }

        public static bool IsAllRadius(this CornerRadius cornerRadius)
        {
            return Math.Abs((
                (cornerRadius.TopLeft +
                cornerRadius.TopRight +
                cornerRadius.BottomLeft +
                cornerRadius.BottomRight) / 4) - cornerRadius.TopLeft) < 0.001;
        }

        public static float[] ToRadii(this ICornerElement cornerElement, double density)
        {
            return cornerElement.CornerRadius.ToRadii(density);
        }

        public static float[] ToRadii(this CornerRadius cornerRadius, double density)
        {
            return new[]
            {
                ToPixels(cornerRadius.TopLeft, density),
                ToPixels(cornerRadius.TopLeft, density),
                ToPixels(cornerRadius.TopRight, density),
                ToPixels(cornerRadius.TopRight, density),
                ToPixels(cornerRadius.BottomRight, density),
                ToPixels(cornerRadius.BottomRight, density),
                ToPixels(cornerRadius.BottomLeft, density),
                ToPixels(cornerRadius.BottomLeft, density)
            };
        }

        public static float ToPixels(double units, double density)
        {
            return (float)(units * density);
        }
    }
}
