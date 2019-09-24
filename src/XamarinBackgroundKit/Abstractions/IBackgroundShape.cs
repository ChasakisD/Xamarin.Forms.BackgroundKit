using System;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Shapes
{
    public interface IBackgroundShape
    {
        bool NeedsBorderInset { get; }
        VisualElement Parent { get; set; }
        IMaterialVisualElement Background { get; }

        event EventHandler<EventArgs> ShapeInvalidateRequested;
    }
}
