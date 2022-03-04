using AndroidX.AppCompat.Widget;
using Xamarin.Forms;
using XamarinBackgroundKit.Android.Effects;
using XamarinBackgroundKit.Effects;

[assembly: ExportEffect(typeof(NoButtonTextCapsEffectDroid), nameof(NoButtonTextCapsEffect))]
namespace XamarinBackgroundKit.Android.Effects
{
    public class NoButtonTextCapsEffectDroid : BasePlatformEffect<NoButtonTextCapsEffect, Element, AppCompatButton>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            
            View?.SetAllCaps(false);
        }

        protected override void OnDetached()
        {
            base.OnDetached();

            if (IsDisposed) return;

            View?.SetAllCaps(true);
        }
    }
}
