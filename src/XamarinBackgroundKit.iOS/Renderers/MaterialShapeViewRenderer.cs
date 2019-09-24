using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.iOS.Renderers;

[assembly: ExportRenderer(typeof(MaterialShapeView), typeof(MaterialShapeViewRenderer))]
namespace XamarinBackgroundKit.iOS.Renderers
{
    public class MaterialShapeViewRenderer : MaterialContentViewRenderer
    {
        private bool _disposed;

        private MaterialShapeView ElementController => Element as MaterialShapeView;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            UpdateShape();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == MaterialShapeView.ShapeProperty.PropertyName) UpdateShape();
        }
        
        private void OnShapeInvalidateRequested(object sender, EventArgs e) => UpdateShape();

        private void UpdateShape()
        {
            if (_disposed) return;

            BackgroundManager?.SetShape(ElementController.Shape);

            SetNeedsLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;

            base.Dispose(disposing);
        }
    }
}