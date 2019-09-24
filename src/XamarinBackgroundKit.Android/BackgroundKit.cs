using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Java.Interop;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Android.PathProviders;
using AApp = Android.App.Application;
using AView = Android.Views.View;

namespace XamarinBackgroundKit.Android
{
    public static class BackgroundKit
    {
        private static double _density;
        internal static double Density
        {
            get
            {
                if (_density > 0) return _density;

                using (var displayMetrics = new DisplayMetrics())
                {
                    var windowService = AApp.Context.GetSystemService(Context.WindowService)
                        ?.JavaCast<IWindowManager>();

                    using (var display = windowService?.DefaultDisplay)
                    {
                        if (display == null) return _density;

                        display.GetRealMetrics(displayMetrics);
                        _density = displayMetrics.Density;
                        return _density;
                    }
                }
            }
        }

        public static readonly PorterDuffXfermode PorterDuffClearMode =
            new PorterDuffXfermode(PorterDuff.Mode.Clear);

        public static readonly PorterDuffXfermode PorterDuffDstInMode =
            new PorterDuffXfermode(PorterDuff.Mode.DstIn);

        public static readonly PorterDuffXfermode PorterDuffDstOutMode =
            new PorterDuffXfermode(PorterDuff.Mode.DstOut);

        public static void Init()
        {
            PathProvidersContainer.Init();
        }
    }
}
