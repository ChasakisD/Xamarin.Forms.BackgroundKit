using System;
using Xamarin.Forms;

namespace XamarinBackgroundKitSample
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void UpdateRippleOnBelowView(object sender, EventArgs e)
        {
            RippleEnabledView.Background.IsRippleEnabled = !RippleEnabledView.Background.IsRippleEnabled;
            
            var r = new Random();

            SimpleView.Background.Elevation = r.Next(0, 4);
            
            var color = Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            RippleEnabledView.Background.Gradients[0].Color = color;

            var borderColor = Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            GradientBorderView.Background.BorderGradients[0].Color = borderColor;
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}
