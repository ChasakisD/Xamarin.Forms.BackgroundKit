using System;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.Shapes
{
    public class BaseShape : BindableObject, IBackgroundShape
    {
        public VisualElement Parent { get; set; }

        public IMaterialVisualElement Background =>
            Parent is IBackgroundElement backgroundElement ? backgroundElement.Background : null;

        public bool NeedsBorderInset =>
            Background != null && Background.BorderStyle == BorderStyle.Outer && Background.BorderWidth > 0;

        public event EventHandler<EventArgs> ShapeInvalidateRequested;

        public virtual void InvalidateShape()
        {
            ShapeInvalidateRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
