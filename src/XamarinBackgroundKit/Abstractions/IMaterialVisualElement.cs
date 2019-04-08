using System.ComponentModel;
using Xamarin.Forms;

namespace XamarinBackgroundKit.Abstractions
{
    public interface IMaterialVisualElement : INotifyPropertyChanged, IBorderElement, ICornerElement, IGradientElement, IElevationElement
    {
        Color Color { get; }
    }
}
