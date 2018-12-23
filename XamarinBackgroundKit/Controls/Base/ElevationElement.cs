using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls.Base
{
	public static class ElevationElement
	{
		public static readonly BindableProperty ElevationProperty = BindableProperty.Create(
			nameof(IElevationElement.Elevation), typeof(double), typeof(IElevationElement), 0d,
			propertyChanged: OnElevationPropertyChanged);

        private static void OnElevationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IElevationElement)bindable).OnElevationPropertyChanged((double)oldValue, (double)newValue);
		}
	}
}
