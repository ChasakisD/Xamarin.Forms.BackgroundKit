using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.iOS.Renderers;

namespace XamarinBackgroundKitSample.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental");
            Xamarin.Forms.Forms.Init();
            
            LoadApplication(new App());

            var types = new[]
            {
                typeof(MaterialCardRenderer),
                typeof(MaterialContentViewRenderer)
            };

            return base.FinishedLaunching(app, options);
        }
    }
}
