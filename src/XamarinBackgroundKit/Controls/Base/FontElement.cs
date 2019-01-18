using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls.Base
{
    public static class FontElement
    {
        public static readonly BindableProperty FontProperty = BindableProperty.Create(
            nameof(IFontElement.Font), typeof(Font), typeof(IFontElement), default(Font),
            propertyChanged: OnFontPropertyChanged);

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            nameof(IFontElement.FontFamily), typeof(string), typeof(IFontElement), default(string),
            propertyChanged: OnFontFamilyChanged);

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
            nameof(IFontElement.FontSize), typeof(double), typeof(IFontElement), -1.0,
            propertyChanged: OnFontSizeChanged, defaultValueCreator: FontSizeDefaultValueCreator);

        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
            nameof(IFontElement.FontAttributes), typeof(FontAttributes), typeof(IFontElement), FontAttributes.None,
            propertyChanged: OnFontAttributesChanged);

        private static readonly BindableProperty CancelEventsProperty = BindableProperty.Create(
            "CancelEvents", typeof(bool), typeof(FontElement), false);

        private static bool GetCancelEvents(BindableObject bindable) => (bool)bindable.GetValue(CancelEventsProperty);

        private static void SetCancelEvents(BindableObject bindable, bool value) => bindable.SetValue(CancelEventsProperty, value);

        private static void OnFontPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (GetCancelEvents(bindable))
                return;

            SetCancelEvents(bindable, true);

            var font = (Font)newValue;
            if (font == Font.Default)
            {
                bindable.ClearValue(FontFamilyProperty);
                bindable.ClearValue(FontSizeProperty);
                bindable.ClearValue(FontAttributesProperty);
            }
            else
            {
                bindable.SetValue(FontFamilyProperty, font.FontFamily);
                bindable.SetValue(FontSizeProperty,
                    font.UseNamedSize ? Device.GetNamedSize(font.NamedSize, bindable.GetType(), true) : font.FontSize);
                bindable.SetValue(FontAttributesProperty, font.FontAttributes);
            }
            SetCancelEvents(bindable, false);
        }

        private static void OnFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (GetCancelEvents(bindable))
                return;

            SetCancelEvents(bindable, true);

            var values = bindable.GetValues(FontSizeProperty, FontAttributesProperty);
            var fontSize = (double)values[0];
            var fontAttributes = (FontAttributes)values[1];
            var fontFamily = (string)newValue;

            bindable.SetValue(FontProperty,
                fontFamily != null
                    ? Font.OfSize(fontFamily, fontSize).WithAttributes(fontAttributes)
                    : Font.SystemFontOfSize(fontSize, fontAttributes));

            SetCancelEvents(bindable, false);
            ((IFontElement)bindable).OnFontFamilyChanged((string)oldValue, (string)newValue);
        }

        private static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (GetCancelEvents(bindable))
                return;

            SetCancelEvents(bindable, true);

            var values = bindable.GetValues(FontFamilyProperty, FontAttributesProperty);
            var fontSize = (double)newValue;
            var fontAttributes = (FontAttributes)values[1];
            var fontFamily = (string)values[0];

            bindable.SetValue(FontProperty,
                fontFamily != null
                    ? Font.OfSize(fontFamily, fontSize).WithAttributes(fontAttributes)
                    : Font.SystemFontOfSize(fontSize, fontAttributes));

            SetCancelEvents(bindable, false);
            ((IFontElement)bindable).OnFontSizeChanged((double)oldValue, (double)newValue);
        }

        private static object FontSizeDefaultValueCreator(BindableObject bindable)
        {
            return ((IFontElement)bindable)?.FontSizeDefaultValueCreator();
        }

        private static void OnFontAttributesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (GetCancelEvents(bindable))
                return;

            SetCancelEvents(bindable, true);

            var values = bindable.GetValues(FontFamilyProperty, FontSizeProperty);
            var fontSize = (double)values[1];
            var fontAttributes = (FontAttributes)newValue;
            var fontFamily = (string)values[0];

            bindable.SetValue(FontProperty,
                fontFamily != null
                    ? Font.OfSize(fontFamily, fontSize).WithAttributes(fontAttributes)
                    : Font.SystemFontOfSize(fontSize, fontAttributes));

            SetCancelEvents(bindable, false);
            ((IFontElement)bindable).OnFontAttributesChanged((FontAttributes)oldValue, (FontAttributes)newValue);
        }
    }
}
