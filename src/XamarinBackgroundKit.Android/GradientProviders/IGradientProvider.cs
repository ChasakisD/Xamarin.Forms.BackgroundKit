using System;
using Android.Graphics;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.Android.GradientProviders
{
    public interface IGradientProvider : IDisposable
    {
        bool HasGradient { get; }

        void SetGradient(GradientBrush gradientBrush);
        void DrawGradient(Paint paint, int width, int height);
        void ClearGradient(Paint paint, int width, int height);
        void DrawOrClearGradient(Paint paint, int width, int height);
    }
}
