using System;
using Android.Graphics;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.Android.PathProviders
{
    public interface IPathProvider : IDisposable
    {
        Path Path { get; }
        bool IsPathDirty { get; }
        bool IsBorderPathDirty { get; }
        bool IsBorderSupported { get; }
        bool CanHandledByOutline { get; }

        void Invalidate();
        void SetShape(IBackgroundShape shape);

        Path CreatePath(int width, int height);
        Path CreateBorderedPath(int width, int height);
    }
}
