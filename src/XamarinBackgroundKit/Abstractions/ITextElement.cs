using Xamarin.Forms;

namespace XamarinBackgroundKit.Abstractions
{
    public interface ITextElement
    {
        string Text { get; }
        Color TextColor { get; }

        void OnTextPropertyChanged(string oldValue, string newValue);
        void OnTextColorPropertyChanged(Color oldValue, Color newValue);
    }
}
