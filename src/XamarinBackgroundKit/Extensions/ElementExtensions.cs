using System;
using System.Linq;
using Xamarin.Forms;

namespace XamarinBackgroundKit.Extensions
{
    public static class ElementExtensions
    {
        public static void AddOrRemoveEffect<T>(this BindableObject bindable, Func<bool> applyEffect) where T : RoutingEffect
        {
            if (!(bindable is Element view)) return;

            var goingToApply = applyEffect();
            var effect = view.Effects?.FirstOrDefault(e => e is T);

            if (goingToApply && effect == null)
            {
                view.Effects?.Add((T)Activator.CreateInstance(typeof(T)));
            }
            else if (effect != null)
            {
                view.Effects?.Remove(effect);
            }
        }
    }
}
