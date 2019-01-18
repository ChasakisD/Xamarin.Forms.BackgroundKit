using System;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls.Base;

namespace XamarinBackgroundKit.Controls.Chips
{
    public class BaseMaterialChip : MaterialContentView, IFontElement, ITextElement
    {
        #region Bindable Properties

        #region ITextElement Properties

        public static readonly BindableProperty TextProperty = TextElement.TextProperty;

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = TextElement.TextColorProperty;

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        #endregion

        #region IFontElement Properties

        public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }

        public static readonly BindableProperty FontProperty = FontElement.FontProperty;

        [Obsolete("Font is obsolete as of version 1.3.0. Please use the Font attributes which are on the class itself.")]
        public Font Font
        {
            get => (Font)GetValue(FontProperty);
            set => SetValue(FontProperty, value);
        }

        #endregion

        #endregion

        #region IFontElement Implementation

        double IFontElement.FontSizeDefaultValueCreator() =>
            Device.GetNamedSize(NamedSize.Default, this);

        void IFontElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue) =>
            InvalidateMeasure();

        void IFontElement.OnFontFamilyChanged(string oldValue, string newValue) =>
            InvalidateMeasure();

        void IFontElement.OnFontSizeChanged(double oldValue, double newValue) =>
            InvalidateMeasure();

        void IFontElement.OnFontChanged(Font oldValue, Font newValue) =>
            InvalidateMeasure();

        #endregion

        #region ITextElement Implementation

        void ITextElement.OnTextPropertyChanged(string oldValue, string newValue) =>
            InvalidateMeasure();

        void ITextElement.OnTextColorPropertyChanged(Color oldValue, Color newValue) { }

        #endregion
    }
}
