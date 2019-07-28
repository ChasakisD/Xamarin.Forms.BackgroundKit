using System;
using Xamarin.Forms;

namespace XamarinBackgroundKitSample.IssuesGalleryViews
{
    public partial class Issue41Page
    {
        private static readonly Random Random = new Random();

        public Issue41Page()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, EventArgs e)
        {
            (sender as View).Margin = new Thickness(Random.Next(200) + 100, 10, 10, 10);
        }
    }
}
