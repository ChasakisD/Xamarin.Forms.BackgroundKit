using Xamarin.Forms;

namespace XamarinBackgroundKitSample
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ExploreViewsPage())
            {
                BarBackgroundColor = Color.White,
                BarTextColor = Color.FromHex("#2D2D2D")
            };
        }
    }
}
