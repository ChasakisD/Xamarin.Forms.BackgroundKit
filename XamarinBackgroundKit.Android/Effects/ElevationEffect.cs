using Xamarin.Forms;
using XamarinBackgroundKit.Android.Effects;
using XamarinBackgroundKit.Android.Extensions;
using XamarinBackgroundKit.Effects;
using AView = Android.Views.View;

[assembly: ExportEffect(typeof(ElevationEffectDroid), nameof(ElevationEffect))]
namespace XamarinBackgroundKit.Android.Effects
{
	public class ElevationEffectDroid : BasePlatformEffect<ElevationEffect, Element, AView>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

            View?.SetElevation(View.Context, Elevation.GetElevation(Element));
		}
	}
}
