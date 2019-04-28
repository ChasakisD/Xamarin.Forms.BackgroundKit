using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.Abstractions
{
    public interface IGradientElement
    {
        LinearGradientBrush GradientBrush { get; }

        void OnGradientBrushPropertyChanged(LinearGradientBrush oldValue, LinearGradientBrush newValue);
    }
}
