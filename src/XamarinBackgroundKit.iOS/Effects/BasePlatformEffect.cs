using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace XamarinBackgroundKit.iOS.Effects
{
    public class BasePlatformEffect<TEffect, TElement, TNativeView> : PlatformEffect
        where TEffect : RoutingEffect
        where TElement : Element
        where TNativeView : UIView
    {
        protected TEffect Effect;
        protected TElement XElement;
        protected TNativeView View;

        protected bool IsDisposed =>
            (Container as IVisualElementRenderer)?.Element == null ||
            View == null || View.Handle == IntPtr.Zero;

        protected override void OnAttached()
        {
            View = (Control ?? Container) as TNativeView;
            XElement = Element as TElement;

            if (Element?.Effects == null || Element.Effects.Count == 0) return;
            Effect = (TEffect)Element.Effects.FirstOrDefault(x => x is TEffect);
        }

        protected override void OnDetached()
        {
            View = (Control ?? Container) as TNativeView;
            XElement = Element as TElement;
        }
    }
}