using XamarinBackgroundKitSample.Models.Issue90;

namespace XamarinBackgroundKitSample.IssuesGalleryViews
{
    public partial class Issue90Page
    {
        public Issue90Page()
        {
            InitializeComponent();

            BindingContext = new Issue90ViewModel();
        }
    }
}
