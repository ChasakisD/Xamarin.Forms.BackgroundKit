using Xamarin.Forms;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.Abstractions
{
    public interface IBorderElement
	{
		Color BorderColor { get; }
		double BorderWidth { get; }
        BorderStyle BorderStyle { get; }

        double DashGap { get; }
        double DashWidth { get; }
        GradientBrush BorderGradientBrush { get; }
        
        void OnBorderColorPropertyChanged(Color oldValue, Color newValue);
		void OnBorderWidthPropertyChanged(double oldValue, double newValue);
        void OnBorderStylePropertyChanged(BorderStyle oldValue, BorderStyle newValue);
        void OnDashGapPropertyChanged(double oldValue, double newValue);
        void OnDashWidthPropertyChanged(double oldValue, double newValue);
        void OnBorderGradientBrushPropertyChanged(GradientBrush oldValue, GradientBrush newValue);
    }
}
