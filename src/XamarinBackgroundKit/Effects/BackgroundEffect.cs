using Xamarin.Forms;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.Effects
{
    public class BackgroundEffect : RoutingEffect
    {
        #region Attached Properties

        public static readonly BindableProperty BackgroundProperty = BindableProperty.CreateAttached(
            "Background", typeof(Background), typeof(BackgroundEffect), new Background(),
            propertyChanged: (b, o, n) => b.AddOrRemoveEffect<BackgroundEffect>(() => true));

        public static Background GetBackground(BindableObject view) => (Background)view.GetValue(BackgroundProperty);

        public static void SetBackground(BindableObject view, Background value) => view.SetValue(BackgroundProperty, value);

        #endregion
        
        public BackgroundEffect() : base("XamarinBackgroundKit.BackgroundEffect") { }
    }
}