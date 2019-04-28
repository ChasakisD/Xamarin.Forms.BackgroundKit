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

        public static readonly BindableProperty DashGapProperty = BindableProperty.Create(
            nameof(IBorderElement.DashGap), typeof(double), typeof(IBorderElement), 0d,
            propertyChanged: OnDashGapPropertyChanged);

        public static readonly BindableProperty DashWidthProperty = BindableProperty.Create(
            nameof(IBorderElement.DashWidth), typeof(double), typeof(IBorderElement), 0d,
            propertyChanged: OnDashWidthPropertyChanged);

        public static readonly BindableProperty BorderGradientBrushProperty = BindableProperty.Create(
            nameof(IBorderElement.BorderGradientBrush), typeof(LinearGradientBrush), typeof(IBorderElement), new LinearGradientBrush(),
            propertyChanged: OnBorderGradientBrushPropertyChanged,
            defaultValueCreator: b => new LinearGradientBrush());
        
        private static void OnBorderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IBorderElement)bindable).OnBorderColorPropertyChanged((Color)oldValue, (Color)newValue);
		}

		private static void OnBorderWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IBorderElement)bindable).OnBorderWidthPropertyChanged((double)oldValue, (double)newValue);
		}

        private static void OnDashGapPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IBorderElement)bindable).OnDashGapPropertyChanged((double)oldValue, (double)newValue);
        }

        private static void OnDashWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IBorderElement)bindable).OnDashWidthPropertyChanged((double)oldValue, (double)newValue);
        }

        private static void OnBorderGradientBrushPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IBorderElement)bindable).OnBorderGradientBrushPropertyChanged((LinearGradientBrush)oldValue, (LinearGradientBrush)newValue);
        }
    }
}
