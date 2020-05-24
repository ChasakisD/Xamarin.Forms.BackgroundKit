using System;
using Xamarin.Forms.Xaml;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.MarkupExtensions
{
    public class BgProviderExtension : Background, IMarkupExtension<Background>
    {
        public Background ProvideValue(IServiceProvider serviceProvider) => this;

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}
