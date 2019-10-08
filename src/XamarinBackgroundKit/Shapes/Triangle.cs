using Xamarin.Forms;

namespace XamarinBackgroundKit.Shapes
{
    public class Triangle : BaseShape
    {
        public static readonly BindableProperty PointAProperty = BindableProperty.Create(
            nameof(PointA), typeof(Point), typeof(Triangle), new Point(),
            propertyChanged: (b, o, n) => ((Triangle)b)?.InvalidateShape());

        public Point PointA
        {
            get => (Point)GetValue(PointAProperty);
            set => SetValue(PointAProperty, value);
        }

        public static readonly BindableProperty PointBProperty = BindableProperty.Create(
            nameof(PointB), typeof(Point), typeof(Triangle), new Point(),
            propertyChanged: (b, o, n) => ((Triangle)b)?.InvalidateShape());

        public Point PointB
        {
            get => (Point)GetValue(PointBProperty);
            set => SetValue(PointBProperty, value);
        }

        public static readonly BindableProperty PointCProperty = BindableProperty.Create(
            nameof(PointC), typeof(Point), typeof(Triangle), new Point(),
            propertyChanged: (b, o, n) => ((Triangle)b)?.InvalidateShape());

        public Point PointC
        {
            get => (Point)GetValue(PointCProperty);
            set => SetValue(PointCProperty, value);
        }
    }
}
