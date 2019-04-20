using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKitSample
{
    public partial class ContentViewExplorerPage
    {
        private int _offsetCount;
        private int _borderOffsetCount;

        private readonly ObservableCollection<GradientStop> _gradientsStackItems;
        private readonly ObservableCollection<GradientStop> _borderGradientsStackItems;

        public ContentViewExplorerPage()
        {
            InitializeComponent();

            _gradientsStackItems = new ObservableCollection<GradientStop>();
            MaterialView.Background.Gradients = _gradientsStackItems;
            BindableLayout.SetItemsSource(GradientsLayout, _gradientsStackItems);

            _borderGradientsStackItems = new ObservableCollection<GradientStop>();
            MaterialView.Background.BorderGradients = _borderGradientsStackItems;
            BindableLayout.SetItemsSource(BorderGradientsLayout, _borderGradientsStackItems);
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
            SyncOffsets(_gradientsStackItems, _offsetCount);
        }
        
        private void OnNewGradientRemoved(object sender, EventArgs e)
        {
            if(_gradientsStackItems.Count <= 0) return;

            _offsetCount--;
            _gradientsStackItems.Remove(_gradientsStackItems.Last());
            SyncOffsets(_gradientsStackItems, _offsetCount);
        }


        private void OnNewBorderGradientAdded(object sender, EventArgs e)
        {
            _borderOffsetCount++;
            _borderGradientsStackItems.Add(new GradientStop());
            SyncOffsets(_borderGradientsStackItems, _borderOffsetCount);
        }

        private void OnNewBorderGradientRemoved(object sender, EventArgs e)
        {
            if (_borderGradientsStackItems.Count <= 0) return;

            _borderOffsetCount--;
            _borderGradientsStackItems.Remove(_borderGradientsStackItems.Last());
            SyncOffsets(_borderGradientsStackItems, _borderOffsetCount);
        }
        
        private void OnGradientChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is BindableObject bindable) || !(bindable.BindingContext is GradientStop gradientStop)) return;

            gradientStop.Color = GetColorFromString(e.NewTextValue);
        }
        
        private static void SyncOffsets(IEnumerable<GradientStop> gradientStops, int offsets)
        {
            var offset = 0f;
            var delta = 1f / (offsets - 1);

            foreach (var gradientStop in gradientStops)
            {
                gradientStop.Offset = offset;
                offset += delta;
            }
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