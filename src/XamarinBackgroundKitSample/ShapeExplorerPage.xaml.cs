using System;
using Xamarin.Forms;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKitSample
{
    public partial class ShapeExplorerPage : ContentPage
    {
        public ShapeExplorerPage()
        {
            InitializeComponent();

            UpdateShape();
        }        

        private void UpdateShape()
        {
            switch(ShapePicker.SelectedItem?.ToString())
            {
                case "Arc":
                    ShapeView.Shape = new Arc();
                    ArcConfigContainer.IsVisible = true;
                    CornerRadiusConfigContainer.IsVisible = false;
                    break;
                case "Rect":
                    ShapeView.Shape = new Rect();
                    ArcConfigContainer.IsVisible = false;
                    CornerRadiusConfigContainer.IsVisible = false;
                    break;
                case "RoundRect":
                    ShapeView.Shape = new RoundRect();
                    ArcConfigContainer.IsVisible = false;
                    CornerRadiusConfigContainer.IsVisible = true;
                    break;
                default:
                    ShapeView.Shape = null;
                    ArcConfigContainer.IsVisible = false;
                    CornerRadiusConfigContainer.IsVisible = false;
                    break;
            }
        }

        private void OnShapeChanged(object sender, EventArgs e)
        {
            UpdateShape();
        }

        private void OnArcPositionChanged(object sender, EventArgs e)
        {
            if (!(ShapeView.Shape is Arc arc)) return;

            arc.Position = (ArcPosition)Enum.Parse(typeof(ArcPosition), ArcPositionPicker.SelectedItem?.ToString());
        }

        private void OnIsCropInsideToggled(object sender, ToggledEventArgs e)
        {
            if (!(ShapeView.Shape is Arc arc)) return;

            arc.IsCropInside = IsCropInsideSwitch.IsToggled;
        }

        private void OnArcHeightChanged(object sender, EventArgs e)
        {
            if (!(ShapeView.Shape is Arc arc)) return;

            arc.ArcHeight = ArcHeightSlider.Value;
        }

        private void OnCornerRadiusChanged(object sender, ValueChangedEventArgs e)
        {
            if (!(ShapeView.Shape is RoundRect roundRect)) return;

            roundRect.CornerRadius = new CornerRadius(TopLeftCornerSlider.Value, TopRightCornerSlider.Value,
                BottomLeftCornerSlider.Value, BottomRightCornerSlider.Value);
        }
    }
}
