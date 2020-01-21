using System;
using CoreGraphics;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.iOS.GradientProviders
{
    public interface IGradientProvider : IDisposable
    {
        bool HasGradient { get; }

        void SetGradient(GradientBrush gradientBrush);
        void DrawGradient(CGContext ctx, CGRect bounds);
        void ClearGradient(CGContext ctx, CGRect bounds);
        void DrawOrClearGradient(CGContext ctx, CGRect bounds);
    }
}
