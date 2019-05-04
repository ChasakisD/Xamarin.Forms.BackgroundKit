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
        private bool _isLayerObserver;
        private IDisposable _layoutChangeObserver;

        private Background _background;
        private MaterialBackgroundManager _backgroundManager;

        protected override void OnAttached()
        {
            base.OnAttached();

            SetTracker();

            _background = BackgroundEffect.GetBackground(Element);
            _background?.SetBinding(BindableObject.BindingContextProperty,
                new Binding("BindingContext", source: Element));

            if (Element is Layout || Container is BoxRenderer)
            {
                if (View?.Layer == null) return;

                if (Container != null)
                {
                    Container.UserInteractionEnabled = true;
                }

                _isLayerObserver = true;
                _layoutChangeObserver = View.Layer.AddObserver(
                    "bounds",
                    NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.OldNew,
                    c => _backgroundManager?.InvalidateLayer());
            }
            else
            {
                if (View == null) return;

                _layoutChangeObserver = View.AddObserver(
                    "frame",
                    NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.OldNew,
                    c => _backgroundManager?.InvalidateLayer());
            }
        }

        protected override void OnDetached()
        {
            if (_background != null)
            {
                if (_layoutChangeObserver is NSObject layoutChangeObserverObject)
                {
                    if (_isLayerObserver)
                    {
                        View?.Layer?.RemoveObserver(layoutChangeObserverObject, "bounds");
                    }
                    else
                    {
                        View?.RemoveObserver(layoutChangeObserverObject, "frame");
                    }
                }

                _backgroundManager?.Dispose();
                _layoutChangeObserver = null;
            }
            
            base.OnDetached();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName != BackgroundEffect.BackgroundProperty.PropertyName) return;

            var oldBackground = _background;

            _background = BackgroundEffect.GetBackground(Element);
            _background?.SetBinding(BindableObject.BindingContextProperty,
                new Binding("BindingContext", source: Element));

            _backgroundManager?.SetBackgroundElement(oldBackground, _background);
        }

        private void SetTracker()
        {
            if (Control is IVisualElementRenderer controlRenderer)
            {
                _backgroundManager = new MaterialBackgroundManager(controlRenderer);
            }
            else if (Container is IVisualElementRenderer containerRenderer)
            {
                _backgroundManager = new MaterialBackgroundManager(containerRenderer);
            }
        }
    }
}