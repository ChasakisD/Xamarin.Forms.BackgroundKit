using XamarinBackgroundKit.iOS.GradientProviders;
using XamarinBackgroundKit.iOS.PathProviders;

namespace XamarinBackgroundKit.iOS
{
    public static class BackgroundKit
    {
        public static void Init()
        {
            PathProvidersContainer.Init();
            GradientProvidersContainer.Init();
        }
    }
}