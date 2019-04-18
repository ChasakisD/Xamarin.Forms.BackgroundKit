using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static readonly BindableProperty BorderAngleProperty = BindableProperty.Create(
            nameof(IBorderElement.BorderAngle), typeof(float), typeof(IBorderElement), 0f,
            propertyChanged: OnBorderAnglePropertyChanged);
        
        public static readonly BindableProperty BorderGradientTypeProperty = BindableProperty.Create(
            nameof(IBorderElement.BorderGradientType), typeof(GradientType), typeof(IBorderElement), GradientType.Linear,
            propertyChanged: OnBorderGradientTypePropertyChanged);
        
        public static readonly BindableProperty BorderGradientsProperty = BindableProperty.Create(
            nameof(IBorderElement.BorderGradients), typeof(IList<GradientStop>), typeof(IBorderElement),
            propertyChanged: OnBorderGradientsPropertyChanged, defaultValueCreator: b => new ObservableCollection<GradientStop>());
        
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

        private static void OnBorderAnglePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IBorderElement)bindable).OnBorderAnglePropertyChanged((float)oldValue, (float)newValue);
        }

        private static void OnBorderGradientTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IBorderElement)bindable).OnBorderGradientTypePropertyChanged((GradientType)oldValue, (GradientType)newValue);
        }

        private static void OnBorderGradientsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IBorderElement)bindable).OnBorderGradientsPropertyChanged((IList<GradientStop>)oldValue, (IList<GradientStop>)newValue);
        }
    }
}
