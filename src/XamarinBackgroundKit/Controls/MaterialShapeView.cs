using System.ComponentModel;
using Xamarin.Forms;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.Controls
{
    [DesignTimeVisible(true)]
    public class MaterialShapeView : MaterialContentView
    {
        public static readonly BindableProperty ShapeProperty = BindableProperty.Create(
            nameof(Shape), typeof(IBackgroundShape), typeof(MaterialShapeView), new Rect(),
            propertyChanged: (b, o, n) =>
            {
                if (!(b is MaterialShapeView shapeView)) return;

                if (o is BaseShape oldShape)
                {
                    oldShape.Parent = null;
                    oldShape.RemoveBinding(BindingContextProperty);
                }

                if (n is BaseShape newShape)
                {
                    newShape.Parent = shapeView;
                    newShape.SetBinding(BindingContextProperty, new Binding("BindingContext", source: shapeView));
                }
            });

        public IBackgroundShape Shape
        {
            get => (IBackgroundShape)GetValue(ShapeProperty);
            set => SetValue(ShapeProperty, value);
        }
    }
}
