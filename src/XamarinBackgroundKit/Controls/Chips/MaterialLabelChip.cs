using Xamarin.Forms;
using TextEffect = XamarinBackgroundKit.Effects.Text;

namespace XamarinBackgroundKit.Controls.Chips
{
    public class MaterialLabelChip : BaseMaterialChip
    {
        private readonly Label _label;

        public MaterialLabelChip()
        {
            _label = new Label
            {
                Text = Text,
                TextColor = TextColor,
                FontSize = FontSize,
                FontFamily = FontFamily,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            TextEffect.SetNoFontPadding(_label, true);

            Content = _label;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == null) return;

            if (propertyName == TextProperty.PropertyName) _label.Text = Text;
            else if (propertyName == TextColorProperty.PropertyName) _label.TextColor = TextColor;
            else if (propertyName == FontSizeProperty.PropertyName) _label.FontSize = FontSize;
            else if (propertyName == FontFamilyProperty.PropertyName) _label.FontFamily = FontFamily;
        }
    }
}
