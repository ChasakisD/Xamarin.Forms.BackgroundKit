using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls.Base
{
    public static class GradientElement
	{
		public static readonly BindableProperty GradientBrushProperty = BindableProperty.Create(
			nameof(IGradientElement.GradientBrush), typeof(LinearGradientBrush), typeof(IGradientElement), new LinearGradientBrush(),
			propertyChanged: OnGradientBrushPropertyChanged,
            defaultValueCreator: b => new LinearGradientBrush());

        private static void OnGradientBrushPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IGradientElement)bindable).OnGradientBrushPropertyChanged((LinearGradientBrush)oldValue, (LinearGradientBrush)newValue);
		}
    }
}
