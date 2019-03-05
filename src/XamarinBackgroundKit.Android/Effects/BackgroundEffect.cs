using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Android.Effects;
using XamarinBackgroundKit.Android.Renderers;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Effects;
using AView = Android.Views.View;

[assembly: ExportEffect(typeof(BackgroundEffectDroid), nameof(BackgroundEffect))]
namespace XamarinBackgroundKit.Android.Effects
{
    public class BackgroundEffectDroid : BasePlatformEffect<BackgroundEffect, Element, AView>
    {
        private Background _background;
        private MaterialVisualElementTracker _tracker;

        protected override void OnAttached()
        {
            base.OnAttached();

            UpdateBackground();
        }

        protected override void OnDetached()
        {
            base.OnDetached();

            UpdateBackground();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == BackgroundEffect.BackgroundProperty.PropertyName) UpdateBackground();
        }

        private void UpdateBackground()
        {
            var oldBackground = _background;

            _background = BackgroundEffect.GetBackground(Element);

            if (Control is IVisualElementRenderer controlRenderer)
            {
                _tracker = new MaterialVisualElementTracker(controlRenderer);
            }
            else if (Container is IVisualElementRenderer containerRenderer)
            {
                _tracker = new MaterialVisualElementTracker(containerRenderer);
            }

            _tracker.SetElement(oldBackground, _background);
        }
    }
}