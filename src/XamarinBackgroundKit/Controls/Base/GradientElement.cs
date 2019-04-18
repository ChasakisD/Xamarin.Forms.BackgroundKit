using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls.Base
{
	public static class GradientElement
	{
		public static readonly BindableProperty AngleProperty = BindableProperty.Create(
			nameof(IGradientElement.Angle), typeof(float), typeof(IGradientElement), 0f,
			propertyChanged: OnAnglePropertyChanged);

        private static void OnAnglePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IGradientElement)bindable).OnAnglePropertyChanged((float)oldValue, (float)newValue);
		}

		public static readonly BindableProperty GradientTypeProperty = BindableProperty.Create(
			nameof(IGradientElement.GradientType), typeof(GradientType), typeof(IGradientElement), GradientType.Linear,
			propertyChanged: OnGradientTypePropertyChanged);

        private static void OnGradientTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IGradientElement)bindable).OnGradientTypePropertyChanged((GradientType)oldValue, (GradientType)newValue);
		}

		public static readonly BindableProperty GradientsProperty = BindableProperty.Create(
			nameof(IGradientElement.Gradients), typeof(IList<GradientStop>), typeof(IGradientElement),
			propertyChanged: OnGradientsPropertyChanged, defaultValueCreator: b => new ObservableCollection<GradientStop>());

        private static void OnGradientsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IGradientElement)bindable).OnGradientsPropertyChanged((IList<GradientStop>)oldValue, (IList<GradientStop>)newValue);
		}
	}
}
