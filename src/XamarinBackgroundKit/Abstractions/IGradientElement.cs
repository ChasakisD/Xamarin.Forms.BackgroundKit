using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.Abstractions
{
    public interface IGradientElement
    {
        GradientBrush GradientBrush { get; }

        void OnGradientBrushPropertyChanged(GradientBrush oldValue, GradientBrush newValue);
    }
}
