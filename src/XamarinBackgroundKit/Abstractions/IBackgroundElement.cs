using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.Abstractions
{
    public interface IBackgroundElement
    {
        Background Background { get; }

        void OnBackgroundChanged(Background oldValue, Background newValue);
    }
}
