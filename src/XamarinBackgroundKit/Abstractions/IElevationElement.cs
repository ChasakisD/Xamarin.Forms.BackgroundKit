using Xamarin.Forms;

namespace XamarinBackgroundKit.Abstractions
{
	public interface IElevationElement
	{
		double Elevation { get; }

        double TranslationZ { get; }

        Color ShadowColor { get; }

        void OnElevationPropertyChanged(double oldValue, double newValue);

        void OnTranslationZPropertyChanged(double oldValue, double newValue);

        void OnShadowColorPropertyChanged(Color oldValue, Color newValue);
    }
}
