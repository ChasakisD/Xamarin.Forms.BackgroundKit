namespace XamarinBackgroundKit.Abstractions
{
	public interface IElevationElement
	{
		double Elevation { get; }

        double TranslationZ { get; }

		void OnElevationPropertyChanged(double oldValue, double newValue);

        void OnTranslationZPropertyChanged(double oldValue, double newValue);
    }
}
