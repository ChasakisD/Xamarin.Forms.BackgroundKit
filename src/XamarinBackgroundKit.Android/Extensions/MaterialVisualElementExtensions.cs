using Android.Content;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Android.Renderers;
using AView = Android.Views.View;

namespace XamarinBackgroundKit.Android.Extensions
{
    public static class MaterialVisualElementExtensions
    {
        public static GradientStrokeDrawable GetBackground(this IMaterialVisualElement materialVisualElement, Context context)
        {
            return new GradientStrokeDrawable.Builder(context)
                .SetMaterialElement(materialVisualElement)
                .Build();
        }
    }
}