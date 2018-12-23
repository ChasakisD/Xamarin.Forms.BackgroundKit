using Android.Widget;
using Xamarin.Forms;
using XamarinBackgroundKit.Android.Effects;
using XamarinBackgroundKit.Effects;

[assembly: ExportEffect(typeof(NoFontPaddingEffectDroid), nameof(NoFontPaddingEffect))]
namespace XamarinBackgroundKit.Android.Effects
{
    public class NoFontPaddingEffectDroid : BasePlatformEffect<NoFontPaddingEffect, Element, TextView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            View?.SetIncludeFontPadding(false);
        }

        protected override void OnDetached()
        {
            base.OnDetached();

            if (IsDisposed) return;

            View?.SetIncludeFontPadding(true);
        }
    }
}
