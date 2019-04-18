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
            RippleEnabledView.Background.Gradients[0].Color =
                Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            GradientBorderView.Background.BorderGradients[0].Color =
                Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
        }
    }
}
