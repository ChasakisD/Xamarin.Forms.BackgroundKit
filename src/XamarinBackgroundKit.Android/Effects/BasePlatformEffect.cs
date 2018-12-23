using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;

namespace XamarinBackgroundKit.Android.Effects
{
    public class BasePlatformEffect<TEffect, TElement, TNativeView> : PlatformEffect
        where TEffect : RoutingEffect
        where TElement : Element
        where TNativeView : AView
    {
        protected TEffect Effect;
        protected TElement XElement;
        protected TNativeView View;

        protected bool IsDisposed => (Container as IVisualElementRenderer)?.Element == null;

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
