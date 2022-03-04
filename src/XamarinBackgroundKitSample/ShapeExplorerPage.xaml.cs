﻿using System;
using Xamarin.Forms;
using XamarinBackgroundKit.Shapes;
using Rect = XamarinBackgroundKit.Shapes.Rect;

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
            ArcConfigContainer.IsVisible = false;
            DiagonalConfigContainer.IsVisible = false;
            CornerRadiusConfigContainer.IsVisible = false;
            CornerClipConfigContainer.IsVisible = false;
            TriangleConfigContainer.IsVisible = false;

            switch (ShapePicker.SelectedItem?.ToString())
            {
                case "Arc":
                    ShapeView.Shape = new Arc();
                    ArcConfigContainer.IsVisible = true;
                    break;
                case "Rect":
                    ShapeView.Shape = new Rect();
                    break;
                case "Circle":
                    ShapeView.Shape = new Circle();
                    break;
                case "RoundRect":
                    ShapeView.Shape = new RoundRect();
                    CornerRadiusConfigContainer.IsVisible = true;
                    break;
                case "Diagonal":
                    ShapeView.Shape = new Diagonal
                    {
                        Angle = 20,
                        Direction = ShapeDirection.Left,
                        Position = ShapePosition.Bottom
                    };
                    DiagonalConfigContainer.IsVisible = true;
                    break;
                case "Triangle":
                    ShapeView.Shape = new Triangle();
                    TriangleConfigContainer.IsVisible = true;
                    break;
                case "CornerClip":
                    ShapeView.Shape = new CornerClip();
                    CornerClipConfigContainer.IsVisible = true;
                    break;
                default:
                    ShapeView.Shape = null;
                    break;
            }
        }

        private void OnShapeChanged(object sender, EventArgs e)
        {
            UpdateShape();
        }

        private void OnPositionChanged(object sender, EventArgs e)
        {
            if (!(sender is Picker picker)) return;
            if (!Enum.TryParse<ShapePosition>(picker.SelectedItem?.ToString(), out var position)) return;

            switch (ShapeView.Shape)
            {
                case Arc arc:
                    arc.Position = position;
                    break;
                case Diagonal diagonal:
                    diagonal.Position = position;
                    break;

            }
        }

        private void OnDirectionChanged(object sender, EventArgs e)
        {
            if (!(sender is Picker picker)) return;
            if (!Enum.TryParse<ShapeDirection>(picker.SelectedItem?.ToString(), out var direction)) return;

            switch (ShapeView.Shape)
            {
                case Diagonal diagonal:
                    diagonal.Direction = direction;
                    break;

            }
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

        private void OnDiagonalAngleChanged(object sender, EventArgs e)
        {
            if (!(ShapeView.Shape is Diagonal diagonal)) return;

            diagonal.Angle = DiagonalAngleSlider.Value;
        }

        private void OnCornerRadiusChanged(object sender, ValueChangedEventArgs e)
        {
            if (!(ShapeView.Shape is RoundRect roundRect)) return;

            roundRect.CornerRadius = new CornerRadius(TopLeftCornerSlider.Value, TopRightCornerSlider.Value,
                BottomLeftCornerSlider.Value, BottomRightCornerSlider.Value);
        }

        private void OnClipSizeChanged(object sender, ValueChangedEventArgs e)
        {
            if (!(ShapeView.Shape is CornerClip cornerClip)) return;

            cornerClip.ClipSize = new CornerRadius(TopLeftClipSizeSlider.Value, TopRightClipSizeSlider.Value,
                BottomLeftClipSizeSlider.Value, BottomRightClipSizeSlider.Value);
        }

        private void OnTrianglePointsChanged(object sender, ValueChangedEventArgs e)
        {
            if (!(ShapeView.Shape is Triangle triangle)) return;

            triangle.PointA = new Point(PointAX.Value, PointAY.Value);
            triangle.PointB = new Point(PointBX.Value, PointBY.Value);
            triangle.PointC = new Point(PointCX.Value, PointCY.Value);
        }
    }
}
