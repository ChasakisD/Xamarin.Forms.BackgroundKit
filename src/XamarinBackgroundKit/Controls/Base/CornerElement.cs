using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls.Base
{
	public static class CornerElement
	{
		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
			nameof(ICornerElement.CornerRadius), typeof(CornerRadius), typeof(ICornerElement), default(CornerRadius),
			propertyChanged: OnCornerRadiusPropertyChanged);

        private static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((ICornerElement)bindable).OnCornerRadiusPropertyChanged((CornerRadius)oldValue, (CornerRadius)newValue);
		}
	}
}
