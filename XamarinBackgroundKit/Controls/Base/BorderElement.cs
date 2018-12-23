using Xamarin.Forms;
using IBorderElement = XamarinBackgroundKit.Abstractions.IBorderElement;

namespace XamarinBackgroundKit.Controls.Base
{
	public static class BorderElement
	{
		public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
			nameof(IBorderElement.BorderColor), typeof(Color), typeof(IBorderElement), Color.Default,
			propertyChanged: OnBorderColorPropertyChanged);

		public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(
			nameof(IBorderElement.BorderWidth), typeof(double), typeof(IBorderElement), 0d,
			propertyChanged: OnBorderWidthPropertyChanged);

		private static void OnBorderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IBorderElement)bindable).OnBorderColorPropertyChanged((Color)oldValue, (Color)newValue);
		}

		private static void OnBorderWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IBorderElement)bindable).OnBorderWidthPropertyChanged((double)oldValue, (double)newValue);
		}
	}
}
