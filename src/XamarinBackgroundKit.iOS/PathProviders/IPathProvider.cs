using System;
using CoreGraphics;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.iOS.PathProviders
{
    public interface IPathProvider : IDisposable
    {
        CGPath Path { get; }
        bool IsPathDirty { get; }
        bool IsBorderPathDirty { get; }
        bool IsBorderSupported { get; }
        bool CanHandledByOutline { get; }

        void Invalidate();
        void SetShape(IBackgroundShape shape);

        CGPath CreatePath(CGRect bounds);
        CGPath CreateBorderedPath(CGRect bounds);
    }
}
