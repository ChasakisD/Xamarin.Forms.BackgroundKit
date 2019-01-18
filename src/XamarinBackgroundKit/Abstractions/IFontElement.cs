using Xamarin.Forms;

namespace XamarinBackgroundKit.Abstractions
{
    public interface IFontElement
    {
        FontAttributes FontAttributes { get; }
        string FontFamily { get; }

        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        double FontSize { get; }

        Font Font { get; }

        void OnFontFamilyChanged(string oldValue, string newValue);
        void OnFontSizeChanged(double oldValue, double newValue);
        double FontSizeDefaultValueCreator();
        void OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue);
        void OnFontChanged(Font oldValue, Font newValue);
    }
}
