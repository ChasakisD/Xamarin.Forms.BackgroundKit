using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinBackgroundKitSample.Models;

namespace XamarinBackgroundKitSample
{
    public partial class ColorPickerPage : IDisposable
    {
        private static TaskCompletionSource<Color?> _taskCompletionSource;

        public static async Task<Color?> ShowAsync()
        {
            if (_taskCompletionSource != null)
            {
                await _taskCompletionSource.Task;
            }

            _taskCompletionSource = new TaskCompletionSource<Color?>();

            await PopupNavigation.Instance.PushAsync(new ColorPickerPage());

            return await _taskCompletionSource.Task;
        }

        private ColorSource _selectedColorSource;

        public ColorPickerPage()
        {
            InitializeComponent();

            var colors = new List<string>
            {
                "#F44336", "#E91E63", "#9C27B0",
                "#673AB7", "#3F51B5", "#2196F3",
                "#03A9F4", "#00BCD4", "#009688",
                "#4CAF50", "#8BC34A", "#CDDC39",
                "#FFEB3B", "#FFC107", "#FF9800",
                "#FF5722", "#795548", "#9E9E9E",
                "#607D8B"
            };

            BindableLayout.SetItemsSource(ColorsLayout, colors.Select(x => new ColorSource(Color.FromHex(x))));
        }
        
        private void OnOkClick(object sender, EventArgs e) => Dismiss();

        private void OnCancelClick(object sender, EventArgs e) => Dispose();

        private void OnBackgroundClick(object sender, EventArgs e) => Dispose();

        private async void OnColorSourceClick(object sender, EventArgs e)
        {
            if (!(sender is BindableObject bindable) || !(bindable.BindingContext is ColorSource selectedColorSource)) return;

            if (_selectedColorSource == selectedColorSource) return;

            selectedColorSource.IsSelected = true;
            if (_selectedColorSource != null)
            {
                _selectedColorSource.IsSelected = false;
            }
            _selectedColorSource = selectedColorSource;

            if (!(sender is ContentView contentView) || !(contentView.Content is Image image)) return;

            image.Opacity = 0;
            await image.FadeTo(1, 600, Easing.CubicOut);
            image.Opacity = 1;
        }

        private async void Dismiss(bool isCancelled = false)
        {
            await PopupNavigation.Instance.PopAsync();

            _taskCompletionSource.TrySetResult(isCancelled ? null : _selectedColorSource?.Color);
        }

        public void Dispose()
        {
            Dismiss(true);
        }
    }
}