using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls.Base;

namespace XamarinBackgroundKit.Shapes
{
    public class RoundRect : BaseShape, ICornerElement
    {
        public static readonly BindableProperty CornerRadiusProperty = CornerElement.CornerRadiusProperty;

		public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public void OnCornerRadiusPropertyChanged(CornerRadius oldValue, CornerRadius newValue) => InvalidateShape();
    }
}
