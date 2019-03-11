using Xamarin.Forms;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.Effects
{
    public static class Text
    {
        #region Bindable Properties
        
        public static readonly BindableProperty NoButtonCapsProperty = BindableProperty.CreateAttached(
            "NoButtonCaps", typeof(bool), typeof(Text), false, propertyChanged: (b, o, n) =>
                b.AddOrRemoveEffect<NoButtonTextCapsEffect>(() => n is bool noCaps && noCaps));

        #endregion

        #region Getters and Setters

        public static bool GetNoButtonCaps(BindableObject view) => (bool)view.GetValue(NoButtonCapsProperty);

        public static void SetNoButtonCaps(BindableObject view, bool value) => view.SetValue(NoButtonCapsProperty, value);

        #endregion       
    }
    
    public class NoButtonTextCapsEffect : RoutingEffect
    {
        public NoButtonTextCapsEffect() : base("XamarinBackgroundKit.NoButtonTextCapsEffect") { }
    }
}
