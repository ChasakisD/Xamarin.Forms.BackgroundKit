using System;
using Xamarin.Forms;

namespace XamarinBackgroundKit.Controls.Chips
{
    public class MaterialButtonChip : BaseMaterialChip
    {
        private readonly Button _button;

        public MaterialButtonChip()
        {
            _button = new Button
            {
                Text = Text,
                TextColor = TextColor,
                FontSize = FontSize,
                FontFamily = FontFamily,
                BorderColor = Color.Transparent,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            _button.Clicked += OnButtonClick;
            _button.Pressed += OnButtonPressed;
            _button.Released += OnButtonReleased;

            Content = _button;
        }

        private void OnButtonClick(object sender, EventArgs e) => OnClicked();

        private void OnButtonPressed(object sender, EventArgs e) => OnPressed();

        private void OnButtonReleased(object sender, EventArgs e) => OnReleased();

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent != null || _button == null) return;
            
            _button.Clicked -= OnButtonClick;
            _button.Pressed -= OnButtonPressed;
            _button.Released -= OnButtonReleased;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == null) return;

            if (propertyName == TextProperty.PropertyName) _button.Text = Text;
            else if (propertyName == TextColorProperty.PropertyName) _button.TextColor = TextColor;
            else if (propertyName == FontSizeProperty.PropertyName) _button.FontSize = FontSize;
            else if (propertyName == FontFamilyProperty.PropertyName) _button.FontFamily = FontFamily;
        }
    }
}
