using Xamarin.Forms;

namespace XamarinBackgroundKitSample
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ContentViewExplorerPage());
        }
    }
}
