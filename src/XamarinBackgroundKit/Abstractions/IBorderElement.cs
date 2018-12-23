using Xamarin.Forms;

namespace XamarinBackgroundKit.Abstractions
{
	public interface IBorderElement
	{
		Color BorderColor { get; }
		double BorderWidth { get; }

		void OnBorderColorPropertyChanged(Color oldValue, Color newValue);
		void OnBorderWidthPropertyChanged(double oldValue, double newValue);
	}
}
