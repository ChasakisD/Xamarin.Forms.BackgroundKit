using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKitSample
{
    public partial class ExploreViewsPage
    {
        public ExploreViewsPage()
        {
            InitializeComponent();

            var customViews = new ObservableCollection<string>
            {
                "MaterialCard",
                "MaterialContentView"
            };

            var xamarinViews = new ObservableCollection<string>
            {
                "ActivityIndicator",
                "BoxView",
                "Button",
                "DatePicker",
                "Editor",
                "Entry",
                "Image",
                "ImageButton",
                "Label",
                "Picker",
                "ProgressBar",
                "SearchBar",
                "Slider",
                "Stepper",
                "Switch",
                "TimePicker",
                "WebView"
            };

            var xamarinLayouts = new ObservableCollection<string>
            {
                "AbsoluteLayout",
                "CarouselView",
                "CollectionView",
                "ContentView",
                "FlexLayout",
                "Frame",
                "Grid",
                "ListView",
                "RelativeLayout",
                "ScrollView",
                "StackLayout"
            };

            CustomViewsCollectionView.ItemsSource = customViews;
            XamarinControlsCollectionView.ItemsSource = xamarinViews;
            XamarinLayoutsCollectionView.ItemsSource = xamarinLayouts;
        }

        private View GetDataTemplate(string control)
        {
            try
            {
                switch (control)
                {
                    case "MaterialCard":
                    case "MaterialContentView":
                        var type = Type.GetType($"XamarinBackgroundKit.Controls.{control}, {typeof(MaterialCard).Assembly.GetName().Name}");
                        if (type == null) return null;
                        return (View)Activator.CreateInstance(type);
                    default:
                        type = Type.GetType($"Xamarin.Forms.{control}, {typeof(Grid).Assembly.GetName().Name}");
                        if (type == null) return null;
                        return (View)Activator.CreateInstance(type);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        private async void OnItemClicked(object sender, EventArgs e)
        {
            if (!(sender is BindableObject bindable) || bindable.BindingContext == null) return;

            var view = GetDataTemplate(bindable.BindingContext.ToString());
            view.Margin = new Thickness(24, 0);
            view.HeightRequest = 120;
            view.HorizontalOptions = LayoutOptions.FillAndExpand;

            await Navigation.PushAsync(new ExplorerPage(view));
        }
    }
}