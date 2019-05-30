using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Abstractions;
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
        private MaterialBackgroundManager _backgroundManager;

        protected override void OnAttached()
        {
            base.OnAttached();

            SetTracker();
            HandleBackgroundManager();

            _background = BackgroundEffect.GetBackground(Element);

            _background?.SetBinding(BindableObject.BindingContextProperty,
                new Binding("BindingContext", source: Element));
        }

        protected override void OnDetached()
        {
            base.OnDetached();

            Dispose();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName != BackgroundEffect.BackgroundProperty.PropertyName) return;

            var oldBackground = _background;

            _background = BackgroundEffect.GetBackground(Element);
            _background?.SetBinding(BindableObject.BindingContextProperty,
                new Binding("BindingContext", source: Element));

            UpdateBackground(oldBackground, _background);
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
        
        private void HandleBackgroundManager()
        {
            /*
            * Hack The Box:
            * For some reason Xamarin.Forms.Platform.Android.BackgroundManager
            * sets the background to null when no BackgroundColor is defined.
            * 
            * So the flow goes this way:
            * 1) The Effect is attached when the Element is set to the Renderer
            * and it is triggered before the BackgroundManager's listener.
            * 2) We set the background of the label
            * 3) The BackgroundManagers' On ElementChanged is triggered
            * 4) BackgroundManager overrides Background
            * 
            * So, in order to prevent that and do not overdraw,
            * we wait till the Control is attached to the Window. 
            * At this time, the BackgroundManager did the override
            * and we are up and running!
            */
            if (!(Element is Image) && !(Element is Label)) return;

            Control.ViewAttachedToWindow += OnAttachedToWindow;
        }

        private void OnAttachedToWindow(object sender, AView.ViewAttachedToWindowEventArgs e)
        {
            UpdateBackground(null, _background);
        }

        private void UpdateBackground(IMaterialVisualElement oldBackground, IMaterialVisualElement newBackground)
        {           
            _backgroundManager?.SetBackgroundElement(oldBackground, newBackground);
        }

        private void Dispose()
        {
            _backgroundManager?.Dispose();

            if (!(Element is Image) && !(Element is Label)) return;

            Control.ViewAttachedToWindow += OnAttachedToWindow;
        }
    }
}