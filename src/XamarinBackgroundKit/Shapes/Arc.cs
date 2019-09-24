using Xamarin.Forms;

namespace XamarinBackgroundKit.Shapes
{
    public class Arc : BaseShape
    {
        public static readonly BindableProperty PositionProperty = BindableProperty.Create(
            nameof(Position), typeof(ShapePosition), typeof(Arc), ShapePosition.Top,
            propertyChanged: (b, o, n) => ((Arc)b)?.InvalidateShape());

        public ShapePosition Position
        {
            get => (ShapePosition)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static readonly BindableProperty IsCropInsideProperty = BindableProperty.Create(
            nameof(IsCropInside), typeof(bool), typeof(Arc), false,
            propertyChanged: (b, o, n) => ((Arc)b)?.InvalidateShape());

        public bool IsCropInside
        {
            get => (bool)GetValue(IsCropInsideProperty);
            set => SetValue(IsCropInsideProperty, value);
        }

        public static readonly BindableProperty ArcHeightProperty = BindableProperty.Create(
            nameof(ArcHeight), typeof(double), typeof(Arc), 0d,
            propertyChanged: (b, o, n) => ((Arc)b)?.InvalidateShape());

        public double ArcHeight
        {
            get => (double)GetValue(ArcHeightProperty);
            set => SetValue(ArcHeightProperty, value);
        }
    }
}
