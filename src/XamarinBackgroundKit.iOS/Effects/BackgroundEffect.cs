using System.ComponentModel;
using FBKVOControllerNS;
using Foundation;
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
        private class KVOObserver : NSObject { }

        private KVOObserver _observer;
        private FBKVOController _kvoController;

        private Background _background;
        private MaterialBackgroundManager _backgroundManager;

        protected override void OnAttached()
        {
            base.OnAttached();

            SetTracker();

            AddViewObservers();

            _background = BackgroundEffect.GetBackground(Element);
            _background?.SetBinding(BindableObject.BindingContextProperty,
                new Binding("BindingContext", source: Element));

            //FIX for Ripple. The views must be focusable
            if (Element is Layout || Container is BoxRenderer)
            {
                if (Container != null)
                {
                    Container.UserInteractionEnabled = true;
                }
            }
        }

        protected override void OnDetached()
        {
            RemoveViewObservers();

            if(_backgroundManager != null)
            {
                _backgroundManager.Dispose();
                _backgroundManager = null;
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

        private void AddViewObservers()
        {
            _observer = new KVOObserver();
            _kvoController = FBKVOController.ControllerWithObserver(_observer);
            _kvoController.Observe(View, "frame", NSKeyValueObservingOptions.OldNew, CallbackFromKVO);
            _kvoController.Observe(View.Layer, "bounds", NSKeyValueObservingOptions.OldNew, CallbackFromKVO);
        }

        private void RemoveViewObservers()
        {
            if(_kvoController != null)
            {
                _kvoController.UnobserveAll();
                _kvoController.Dispose();
                _kvoController = null;
            }

            if(_observer != null)
            {
                _observer.Dispose();
                _observer = null;
            }
        }

        private void CallbackFromKVO(NSObject observer, NSObject observed, NSDictionary<NSString, NSObject> change)
        {
            _backgroundManager?.InvalidateLayer();
        }
    }
}