using Xamarin.Forms;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.Effects
{
    public static class Elevation
    {
        #region Bindable Properties

        public static readonly BindableProperty ElevationProperty =
            BindableProperty.CreateAttached(
                "Elevation", typeof(float), typeof(Elevation), 0f, propertyChanged: (b, o, n) =>
                    b.AddOrRemoveEffect<ElevationEffect>(() => n is float elevation && elevation > 0));

        #endregion

        #region Getters and Setters

        public static float GetElevation(BindableObject view)
        {
            return (float)view.GetValue(ElevationProperty);
        }

        public static void SetElevation(BindableObject view, float value)
        {
            view.SetValue(ElevationProperty, value);
        }

        #endregion
    }

    public class ElevationEffect : RoutingEffect
    {
        public ElevationEffect() : base("XamarinBackgroundKit.ElevationEffect") { }
    }
}
