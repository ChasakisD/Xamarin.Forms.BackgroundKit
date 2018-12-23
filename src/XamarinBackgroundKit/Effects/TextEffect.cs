using Xamarin.Forms;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.Effects
{
    public static class Text
    {
        #region Bindable Properties

        public static readonly BindableProperty NoFontPaddingProperty = BindableProperty.CreateAttached(
            "NoFontPadding", typeof(bool), typeof(Text), false, propertyChanged: (b, o, n) =>
                b.AddOrRemoveEffect<NoFontPaddingEffect>(() => n is bool noFontPadding && noFontPadding));

        public static readonly BindableProperty NoButtonCapsProperty = BindableProperty.CreateAttached(
            "NoButtonCaps", typeof(bool), typeof(Text), false, propertyChanged: (b, o, n) =>
                b.AddOrRemoveEffect<NoButtonTextCapsEffect>(() => n is bool noCaps && noCaps));

        #endregion

        #region Getters and Setters

        public static bool GetNoFontPadding(BindableObject view) => (bool)view.GetValue(NoFontPaddingProperty);

        public static void SetNoFontPadding(BindableObject view, bool value) => view.SetValue(NoFontPaddingProperty, value);

        public static bool GetNoButtonCaps(BindableObject view) => (bool)view.GetValue(NoButtonCapsProperty);

        public static void SetNoButtonCaps(BindableObject view, bool value) => view.SetValue(NoButtonCapsProperty, value);

        #endregion       
    }

    public class NoFontPaddingEffect : RoutingEffect
    {
        public NoFontPaddingEffect() : base("XamarinBackgroundKit.NoFontPaddingEffect") { }
    }

    public class NoButtonTextCapsEffect : RoutingEffect
    {
        public NoButtonTextCapsEffect() : base("XamarinBackgroundKit.NoButtonTextCapsEffect") { }
    }
}
