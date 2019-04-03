using Foundation;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Effects;
using XamarinBackgroundKit.iOS.Effects;
using XamarinBackgroundKit.iOS.Renderers;

[assembly: ExportEffect(typeof(BackgroundEffectiOS), nameof(BackgroundEffect))]
namespace XamarinBackgroundKit.iOS.Effects
{
    public class BackgroundEffectiOS : BasePlatformEffect<BackgroundEffect, Element, UIView>
    {
        private IDisposable _frameObserver;

        private Background _background;
        private MaterialVisualElementTracker _tracker;

        protected override void OnAttached()
        {
            base.OnAttached();

            SetTracker();

            _background = BackgroundEffect.GetBackground(Element);

            UpdateBackground(null, _background);

            if (View?.Layer == null) return;

            _frameObserver = View.AddObserver(
                "frame",
                NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.OldNew,
                c => _tracker?.InvalidateLayer());
        }

        protected override void OnDetached()
        {
            base.OnDetached();

            if (View != null && _frameObserver is NSObject frameObserverObject)
            {
                View.RemoveObserver(frameObserverObject, "frame");
            }

            _tracker?.Dispose();
            _frameObserver?.Dispose();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == BackgroundEffect.BackgroundProperty.PropertyName)
            {
                var oldBackground = _background;

                _background = BackgroundEffect.GetBackground(Element);

                UpdateBackground(oldBackground, _background);
            }
        }

        private void SetTracker()
        {
            if (Control is IVisualElementRenderer controlRenderer)
            {
                _tracker = new MaterialVisualElementTracker(controlRenderer);
            }
            else if (Container is IVisualElementRenderer containerRenderer)
            {
                _tracker = new MaterialVisualElementTracker(containerRenderer);
            }
        }

        private void UpdateBackground(Background oldBackground, Background newBackground)
        {
            _tracker?.SetElement(oldBackground, newBackground);
        }
    }
}