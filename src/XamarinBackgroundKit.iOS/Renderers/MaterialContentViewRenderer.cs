using System.ComponentModel;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.iOS.Renderers;

[assembly: ExportRenderer(typeof(MaterialContentView), typeof(MaterialContentViewRenderer))]

namespace XamarinBackgroundKit.iOS.Renderers
{
    public class MaterialContentViewRenderer : ViewRenderer
    {
        private bool _disposed;
        protected MaterialBackgroundManager BackgroundManager;

        private MaterialContentView ElementController => Element as MaterialContentView;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            BackgroundManager?.InvalidateLayer();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null) return;

            if (BackgroundManager == null)
            {
                BackgroundManager = new MaterialBackgroundManager(this);
            }

            UpdateIsFocusable();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_disposed) return;

            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == MaterialContentView.IsFocusableProperty.PropertyName
                || e.PropertyName == MaterialContentView.IsClickableProperty.PropertyName) UpdateIsFocusable();
        }

        private void UpdateIsFocusable()
        {
            if ((ElementController?.IsFocusable ?? false) && (ElementController?.IsClickable ?? false))
            {
                //MultipleTouchEnabled = true;
                UserInteractionEnabled = true;
            }
            else
            {
                //MultipleTouchEnabled = false;
                UserInteractionEnabled = false;
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if (ElementController?.IsFocusable ?? false)
            {
                ElementController?.OnPressed();
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            if (ElementController?.IsClickable ?? false)
            {
                ElementController?.OnClicked();
            }

            if (ElementController?.IsFocusable ?? false)
            {
                ElementController?.OnReleased();
                ElementController?.OnReleasedOrCancelled();
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            if (ElementController?.IsFocusable ?? false)
            {
                ElementController?.OnCancelled();
                ElementController?.OnReleasedOrCancelled();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;

            if (disposing)
            {
                if (BackgroundManager != null)
                {
                    BackgroundManager?.Dispose();
                    BackgroundManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}