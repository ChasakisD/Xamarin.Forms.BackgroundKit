using System;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.Shapes
{
    public class BaseShape : BindableObject, IBackgroundShape
    {
        private WeakReference<VisualElement> _parentWeakRef;
        private IMaterialVisualElement Background
        {
            get
            {
                if (_parentWeakRef == null) return null;
                if (!_parentWeakRef.TryGetTarget(out var parent)) return null;

                return parent is IBackgroundElement backgroundElement
                    ? backgroundElement.Background
                    : null;
            }
        }

        public double BorderWidth => Background?.BorderWidth ?? 0d;
        public bool NeedsBorderInset => BorderWidth > 0 && Background.BorderStyle == BorderStyle.Outer;

        public event EventHandler<EventArgs> ShapeInvalidateRequested;

        public virtual void InvalidateShape()
        {
            ShapeInvalidateRequested?.Invoke(this, EventArgs.Empty);
        }

        public void SetParent(VisualElement parent)
        {
            _parentWeakRef = new WeakReference<VisualElement>(parent);
        }
    }
}
