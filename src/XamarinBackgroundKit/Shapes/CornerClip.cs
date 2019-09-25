using Xamarin.Forms;

namespace XamarinBackgroundKit.Shapes
{
    public class CornerClip : BaseShape
    {
        public static readonly BindableProperty ClipSizeProperty = BindableProperty.Create(
        nameof(ClipSize), typeof(CornerRadius), typeof(CornerClip), new CornerRadius(0d),
        propertyChanged: (b, o, n) => ((CornerClip)b)?.InvalidateShape(),
        defaultValueCreator: b => new CornerRadius(0d));

        public CornerRadius ClipSize
        {
            get => (CornerRadius)GetValue(ClipSizeProperty);
            set => SetValue(ClipSizeProperty, value);
        }
    }
}
