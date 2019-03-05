using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls.Base
{
    public static class BackgroundElement
    {
        public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(
            nameof(IBackgroundElement.Background), typeof(Background), typeof(IBackgroundElement),
            defaultValueCreator: b => new Background(), propertyChanged: OnBackgroundPropertyChanged);

        private static void OnBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IBackgroundElement)bindable).OnBackgroundChanged((Background)oldValue, (Background)newValue);
        }
    }
}
