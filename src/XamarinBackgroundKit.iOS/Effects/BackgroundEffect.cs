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
        private IDisposable _layoutChangeObserver;

        private Background _background;
        private MaterialVisualElementTracker _tracker;

        protected override void OnAttached()
        {
            base.OnAttached();

            SetTracker();

            _background = BackgroundEffect.GetBackground(Element);
            _background.SetBinding(BindableObject.BindingContextProperty,
                new Binding("BindingContext", source: Element));

            UpdateBackground(null, _background);

            if (View?.Layer == null) return;

            if (Element is Layout)
            {
                _layoutChangeObserver = View.Layer.AddObserver(
                    "bounds",
                    NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.OldNew,
                    c => _tracker?.InvalidateLayer());
            }
            else
            {
                _layoutChangeObserver = View.AddObserver(
                    "frame",
                    NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.OldNew,
                    c => _tracker?.InvalidateLayer());
            }
        }

        protected override void OnDetached()
        {
            if (_layoutChangeObserver is NSObject layoutChangeObserverObject)
            {
                if (Element is Layout)
                {
                    View?.Layer?.RemoveObserver(layoutChangeObserverObject, "bounds");
                }
                else
                {
                    View?.RemoveObserver(layoutChangeObserverObject, "frame");
                }
            }
            
            _tracker?.Dispose();
            _layoutChangeObserver?.Dispose();
            
            base.OnDetached();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == BackgroundEffect.BackgroundProperty.PropertyName)
            {
                var oldBackground = _background;

                _background = BackgroundEffect.GetBackground(Element);
                _background.SetBinding(BindableObject.BindingContextProperty,
                    new Binding("BindingContext", source: Element));

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