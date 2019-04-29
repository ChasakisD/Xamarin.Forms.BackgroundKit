using Xamarin.Forms;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Effects;

namespace XamarinBackgroundKit.Extensions
{
    public static class BackgroundExtensions
    {
        public static T ApplyBackground<T>(this T element, Background background) where T : Element
        {
            BackgroundEffect.SetBackground(element, background);
            return element;
        }
    }
}
