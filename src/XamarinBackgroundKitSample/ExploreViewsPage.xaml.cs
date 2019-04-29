using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;

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
                "MaterialContentView",
                "MaterialCardShowCase1"
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
                    case "MaterialCardShowCase1":
                        return GetMaterialShowCase1();
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
            if (view is ContentView contentView && contentView.Content == null)
            {
                view.HeightRequest = 120;
            }

            view.Margin = new Thickness(24, 0);
            view.HorizontalOptions = LayoutOptions.FillAndExpand;

            await Navigation.PushAsync(new ExplorerPage(view));
        }

        private View GetMaterialShowCase1() =>
            new MaterialContentView
            {
                Content = new StackLayout
                {
                    Spacing = 0,
                    Children =
                    {
                        new Image
                        {
                            HeightRequest = 194,
                            Aspect = Aspect.AspectFill,
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Source = new UriImageSource
                            {
                                Uri = new Uri(
                                    "https://devblogs.microsoft.com/xamarin/wp-content/uploads/sites/44/2018/11/Codemonkeys2.jpg")
                            }
                        },
                        new Label
                        {
                            FontSize = 22,
                            Text = "Material Card",
                            TextColor = Color.FromHex("4A4A4A"),
                            Margin = new Thickness(16, 16, 0, 4)
                        },
                        new Label
                        {
                            FontSize = 16,
                            Text = "Xamarin Forms Material Showcase",
                            TextColor = Color.Gray,
                            Margin = new Thickness(16, 0, 0, 4)
                        },
                        new Label
                        {
                            FontSize = 14,
                            Text = "Welcome to Xamarin.Forms Background Kit Material ShowCase",
                            TextColor = Color.Gray,
                            Margin = new Thickness(16, 16, 24, 4)
                        },
                        new StackLayout
                        {
                            Margin = new Thickness(16),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                new Button
                                {
                                    TextColor = Color.Purple,
                                    FontSize = 18,
                                    Text = "Action",
                                    FontAttributes = FontAttributes.Bold,
                                    BackgroundColor = Color.Transparent
                                }
                            }
                        }
                    }
                }
            };
    }
}