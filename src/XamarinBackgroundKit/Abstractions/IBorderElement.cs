using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.Abstractions
{
	public interface IBorderElement
	{
		Color BorderColor { get; }
		double BorderWidth { get; }
        double DashGap { get; }
        double DashWidth { get; }
        float BorderAngle { get; }
        GradientType BorderGradientType { get; }
        IList<GradientStop> BorderGradients { get; }

        event EventHandler<EventArgs> InvalidateBorderGradientRequested;

        void OnBorderColorPropertyChanged(Color oldValue, Color newValue);
		void OnBorderWidthPropertyChanged(double oldValue, double newValue);
        void OnDashGapPropertyChanged(double oldValue, double newValue);
        void OnDashWidthPropertyChanged(double oldValue, double newValue);
        void OnBorderAnglePropertyChanged(float oldValue, float newValue);
        void OnBorderGradientTypePropertyChanged(GradientType oldValue, GradientType newValue);
        void OnBorderGradientsPropertyChanged(IList<GradientStop> oldValue, IList<GradientStop> newValue);
    }
}
