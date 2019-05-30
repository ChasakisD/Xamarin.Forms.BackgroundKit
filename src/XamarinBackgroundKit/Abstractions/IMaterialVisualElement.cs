using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace XamarinBackgroundKit.Abstractions
{
    public interface IMaterialVisualElement : INotifyPropertyChanged, IBorderElement, ICornerElement, IGradientElement, IElevationElement
    {
        Color Color { get; }
        
        bool IsRippleEnabled { get; }
        
        Color RippleColor { get; }

        bool IsClippedToBounds { get; }

        event EventHandler<EventArgs> InvalidateGradientRequested;

        event EventHandler<EventArgs> InvalidateBorderGradientRequested;
    }
}
