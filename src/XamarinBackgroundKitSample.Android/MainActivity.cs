using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Android;
using XEPlatform = Xamarin.Essentials.Platform;

namespace XamarinBackgroundKitSample.Droid
{
    [Activity(
        Label = "XamarinBackgroundKitSample", 
        Icon = "@mipmap/icon", 
        Theme = "@style/MainTheme.Launcher", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            BackgroundKit.Init();
            XEPlatform.Init(this, savedInstanceState);

            Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental", "FastRenderers_Experimental");
            Forms.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            XEPlatform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}