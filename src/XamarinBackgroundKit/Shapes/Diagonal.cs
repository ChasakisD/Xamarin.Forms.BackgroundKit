using System;
using Xamarin.Forms;

namespace XamarinBackgroundKit.Shapes
{
    public class Diagonal : BaseShape
    {
        public static readonly BindableProperty PositionProperty = BindableProperty.Create(
            nameof(Position), typeof(ShapePosition), typeof(Diagonal), ShapePosition.Top,
            propertyChanged: (b, o, n) => ((Diagonal)b)?.InvalidateShape());

        public ShapePosition Position
        {
            get => (ShapePosition)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static readonly BindableProperty DirectionProperty = BindableProperty.Create(
            nameof(Direction), typeof(ShapeDirection), typeof(Diagonal), ShapeDirection.Left,
            propertyChanged: (b, o, n) => ((Diagonal)b)?.InvalidateShape());

        public ShapeDirection Direction
        {
            get => (ShapeDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        public static readonly BindableProperty AngleProperty = BindableProperty.Create(
            nameof(Angle), typeof(double), typeof(Diagonal), 0d,
            propertyChanged: (b, o, n) => ((Diagonal)b)?.InvalidateShape());

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }
    }
}
