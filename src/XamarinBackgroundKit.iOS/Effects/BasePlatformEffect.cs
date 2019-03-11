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

        protected bool IsDisposed => (Container as IVisualElementRenderer)?.Element == null || View.Handle == IntPtr.Zero;

        protected override void OnAttached() => SetUpEffectProperties();

        protected override void OnDetached() => SetUpEffectProperties();

        private void SetUpEffectProperties()
        {
            View = (Control ?? Container) as TNativeView;
            XElement = Element as TElement;

            if (Element?.Effects == null || Element.Effects.Count == 0) return;
            Effect = Element.Effects.FirstOrDefault(x => x is TEffect) as TEffect;
        }
    }
}