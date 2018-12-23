using System.Collections.Generic;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.Abstractions
{
    public interface IGradientElement
    {
        float Angle { get; }
        GradientType GradientType { get; }
        IList<GradientStop> Gradients { get; }

        void OnAnglePropertyChanged(float oldValue, float newValue);
        void OnGradientTypePropertyChanged(GradientType oldValue, GradientType newValue);
        void OnGradientsPropertyChanged(IList<GradientStop> oldValue, IList<GradientStop> newValue);
    }
}
