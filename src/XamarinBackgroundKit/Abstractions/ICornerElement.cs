using Xamarin.Forms;

namespace XamarinBackgroundKit.Abstractions
{
    public interface ICornerElement
    {
        CornerRadius CornerRadius { get; }

        void OnCornerRadiusPropertyChanged(CornerRadius oldValue, CornerRadius newValue);
    }
}
