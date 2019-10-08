using System;
using Xamarin.Forms;

namespace XamarinBackgroundKit.Shapes
{
    public interface IBackgroundShape
    {
        double BorderWidth { get; }
        bool NeedsBorderInset { get; }

        event EventHandler<EventArgs> ShapeInvalidateRequested;

        void SetParent(VisualElement parent);
    }
}
