using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.iOS;

namespace XamarinBackgroundKitSample.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            BackgroundKit.Init();

            Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental", "FastRenderers_Experimental");
            Forms.Init();
            FormsMaterial.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
