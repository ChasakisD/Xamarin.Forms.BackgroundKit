using Android.Graphics;

namespace XamarinBackgroundKit.Android
{
    public static class BackgroundKit
    {
        public static readonly PorterDuffXfermode PorterDuffClearMode =
            new PorterDuffXfermode(PorterDuff.Mode.Clear);

        public static void Init() { }
    }
}