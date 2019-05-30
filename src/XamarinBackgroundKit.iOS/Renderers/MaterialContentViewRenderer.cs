using Foundation;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.iOS.Renderers;

[assembly: ExportRenderer(typeof(MaterialContentView), typeof(MaterialContentViewRenderer))]
namespace XamarinBackgroundKit.iOS.Renderers
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class MaterialContentViewRenderer : ViewRenderer
    {
        private bool _disposed;
        private MaterialBackgroundManager _materialBackgroundManager;

        private MaterialContentView ElementController => Element as MaterialContentView;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _materialBackgroundManager?.InvalidateLayer();
        }

        #region Element Changed

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null) return;

            if (_materialBackgroundManager == null)
            {
                _materialBackgroundManager = new MaterialBackgroundManager(this);
            }

            UpdateIsFocusable();
        }

        #endregion

        #region Property Changed

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_disposed) return;

            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == MaterialContentView.IsFocusableProperty.PropertyName
                || e.PropertyName == MaterialContentView.IsClickableProperty.PropertyName) UpdateIsFocusable();
        }

        private void UpdateIsFocusable()
        {
            if (ElementController.IsFocusable && ElementController.IsClickable)
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

        #endregion

        #region Touch Handling

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if (ElementController.IsFocusable)
            {
                ElementController?.OnPressed();
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            if (ElementController.IsClickable)
            {
                ElementController?.OnClicked();
            }

            if (ElementController.IsFocusable)
            {
                ElementController?.OnReleased();
                ElementController?.OnReleasedOrCancelled();
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            if (ElementController.IsFocusable)
            {
                ElementController?.OnCancelled();
                ElementController?.OnReleasedOrCancelled();
            }
        }

        #endregion

        #region LifeCycle

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;

            if (disposing)
            {
                if (_materialBackgroundManager != null)
                {
                    _materialBackgroundManager.Dispose();
                    _materialBackgroundManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}