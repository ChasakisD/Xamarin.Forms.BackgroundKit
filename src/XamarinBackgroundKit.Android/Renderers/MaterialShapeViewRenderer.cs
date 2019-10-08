using System.ComponentModel;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Android.Renderers;
using XamarinBackgroundKit.Controls;
using View = Xamarin.Forms.View;

[assembly: ExportRenderer(typeof(MaterialShapeView), typeof(MaterialShapeViewRenderer))]
namespace XamarinBackgroundKit.Android.Renderers
{
    public class MaterialShapeViewRenderer : MaterialContentViewRenderer
    {
        private MaterialShapeView ElementController => Element as MaterialShapeView;

        public MaterialShapeViewRenderer(Context context) : base(context) { }

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

        private void UpdateShape()
        {
            BackgroundManager.SetShape(ElementController.Shape);
        }
    }
}
