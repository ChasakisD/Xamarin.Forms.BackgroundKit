using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls.Base
{
	public static class ElevationElement
	{
		public static readonly BindableProperty ElevationProperty = BindableProperty.Create(
			nameof(IElevationElement.Elevation), typeof(double), typeof(IElevationElement), 0d,
			propertyChanged: OnElevationPropertyChanged);

        public static readonly BindableProperty ShadowColorProperty = BindableProperty.Create(
            nameof(IElevationElement.ShadowColor), typeof(Color), typeof(IElevationElement), Color.Black,
            propertyChanged: OnShadowColorPropertyChanged);

        public static readonly BindableProperty TranslationZProperty = BindableProperty.Create(
            nameof(IElevationElement.TranslationZ), typeof(double), typeof(IElevationElement), 0d,
            propertyChanged: OnTranslationZPropertyChanged);

        private static void OnElevationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IElevationElement)bindable).OnElevationPropertyChanged((double)oldValue, (double)newValue);
		}

        private static void OnShadowColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IElevationElement)bindable).OnShadowColorPropertyChanged((Color)oldValue, (Color)newValue);
        }

        private static void OnTranslationZPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IElevationElement)bindable).OnTranslationZPropertyChanged((double)oldValue, (double)newValue);
        }
    }
}
