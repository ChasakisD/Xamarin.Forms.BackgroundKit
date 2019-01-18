using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls.Base
{
    public static class TextElement
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(ITextElement.Text), typeof(string), typeof(ITextElement), default(string),
            propertyChanged: OnTextPropertyChanged);

        private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ITextElement)bindable).OnTextPropertyChanged((string)oldValue, (string)newValue);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(ITextElement.TextColor), typeof(Color), typeof(ITextElement), Color.Default,
            propertyChanged: OnTextColorPropertyChanged);

        private static void OnTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ITextElement)bindable).OnTextColorPropertyChanged((Color)oldValue, (Color)newValue);
        }
    }
}
