namespace XamarinBackgroundKit.Abstractions
{
	public interface IElevationElement
	{
		double Elevation { get; }

		void OnElevationPropertyChanged(double oldValue, double newValue);
	}
}
