using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKitSample
{
    public partial class ContentViewExplorerPage
    {
        private int _offsetCount;

        private ObservableCollection<GradientStop> _gradientsStackItems;

        public ContentViewExplorerPage()
        {
            InitializeComponent();

            _gradientsStackItems = new ObservableCollection<GradientStop>();
            BindableLayout.SetItemsSource(GradientsLayout, _gradientsStackItems);
        }

        private void OnBackgroundColorChanged(object sender, TextChangedEventArgs e)
        {
            MaterialView.Background.Color = GetColorFromString(e.NewTextValue);
        }

        private void OnBorderColorChanged(object sender, TextChangedEventArgs e)
        {
            MaterialView.Background.BorderColor = GetColorFromString(e.NewTextValue);
        }

        private void OnRippleColorChanged(object sender, TextChangedEventArgs e)
        {
            MaterialView.Background.RippleColor = GetColorFromString(e.NewTextValue);
        }
        
        private void OnNewGradientAdded(object sender, EventArgs e)
        {
            _offsetCount++;
            _gradientsStackItems.Add(new GradientStop());
            SyncOffsets();
        }
        
        private void OnNewGradientRemoved(object sender, EventArgs e)
        {
            if(_gradientsStackItems.Count <= 0) return;

            _offsetCount--;
            _gradientsStackItems.Remove(_gradientsStackItems.Last());
            SyncOffsets();
        }

        private void SyncOffsets()
        {
            var offset = 0f;
            var delta = 1f / _offsetCount;

            foreach (var gradientStop in _gradientsStackItems)
            {
                gradientStop.Offset = offset;
                offset += delta;
            }
        }

        private void OnGradientChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is BindableObject bindable) || !(bindable.BindingContext is GradientStop gradientStop)) return;

            gradientStop.Color = GetColorFromString(e.NewTextValue);
        }

        private static Color GetColorFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return Color.Default;

            try
            {
                return Color.FromHex(value[0].Equals('#') ? value : $"#{value}");
            }
            catch (Exception)
            {
                return Color.Default;
            }
        }
    }
}