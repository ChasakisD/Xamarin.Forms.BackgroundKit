using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Extensions
{
    public static class CornerRadiusExtensions
    {
        public static float[] ToRadii(this ICornerElement cornerElement, double density)
        {
            return cornerElement.CornerRadius.ToRadii(density);
        }

        public static float[] ToRadii(this CornerRadius cornerRadius, double density)
        {
            var (topLeft, topRight, bottomLeft, bottomRight) = cornerRadius;

            return new[]
            {
                ToPixels(topLeft, density),
                ToPixels(topLeft, density),
                ToPixels(topRight, density),
                ToPixels(topRight, density),
                ToPixels(bottomRight, density),
                ToPixels(bottomRight, density),
                ToPixels(bottomLeft, density),
                ToPixels(bottomLeft, density)
            };
        }

        public static float ToPixels(double units, double density)
        {
            return (float)(units * density);
        }
    }
}
