using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;

namespace XamarinBackgroundKit.Android.Extensions
{
    public static class VisualElementRendererExtensions
    {
        public static AView ResolveViewFromRenderer(this IVisualElementRenderer renderer)
        {
            switch (renderer)
            {
                case EntryRenderer entryRenderer:
                    return entryRenderer.Control;
                case IButtonLayoutRenderer buttonLayoutRenderer:
                    return buttonLayoutRenderer.View;
                case IBorderVisualElementRenderer borderVisualElementRenderer:
                    return borderVisualElementRenderer.View;
                default:
                    return renderer.View;
            }
        }
    }
}
