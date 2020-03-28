using System.Windows.Input;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls.Base
{
	public static class ClickableElement
	{
		public static readonly BindableProperty IsClickableProperty = BindableProperty.Create(
			nameof(IClickableElement.IsClickable), typeof(bool), typeof(IClickableElement), true,
			propertyChanged: OnIsClickablePropertyChanged);

		public static readonly BindableProperty ClickedCommandProperty = BindableProperty.Create(
			nameof(IClickableElement.ClickedCommand), typeof(ICommand), typeof(IClickableElement));

		public static readonly BindableProperty ClickedCommandParameterProperty = BindableProperty.Create(
			nameof(IClickableElement.ClickedCommandParameter), typeof(object), typeof(IClickableElement));

        private static void OnIsClickablePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IClickableElement)bindable).OnIsClickablePropertyChanged((bool)oldValue, (bool)newValue);
		}
	}
}
