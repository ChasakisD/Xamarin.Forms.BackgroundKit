using Xamarin.Forms;

namespace XamarinBackgroundKitSample
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            Device.SetFlags(new[] {
                "SwipeView_Experimental",
                "CarouselView_Experimental",
                "IndicatorView_Experimental"
            });

            MainPage = new NavigationPage(new ExploreViewsPage())
            {
                BarBackgroundColor = Color.White,
                BarTextColor = Color.FromHex("#2D2D2D")
            };
        }
    }
}
