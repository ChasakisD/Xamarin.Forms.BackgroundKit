using System.Windows.Input;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls.Base
{
	public static class FocusableElement
	{
		public static readonly BindableProperty IsFocusableProperty = BindableProperty.Create(
			nameof(IFocusableElement.IsFocusable), typeof(bool), typeof(IFocusableElement), true,
			propertyChanged: OnIsFocusablePropertyChanged);

		public static readonly BindableProperty PressedCommandProperty = BindableProperty.Create(
			nameof(IFocusableElement.PressedCommand), typeof(ICommand), typeof(IFocusableElement));

		public static readonly BindableProperty PressedCommandParameterProperty = BindableProperty.Create(
			nameof(IFocusableElement.PressedCommandParameter), typeof(object), typeof(IFocusableElement));

		public static readonly BindableProperty ReleasedCommandProperty = BindableProperty.Create(
			nameof(IFocusableElement.ReleasedCommand), typeof(ICommand), typeof(IFocusableElement));

		public static readonly BindableProperty ReleasedCommandParameterProperty = BindableProperty.Create(
			nameof(IFocusableElement.ReleasedCommandParameter), typeof(object), typeof(IFocusableElement));

		public static readonly BindableProperty CancelledCommandProperty = BindableProperty.Create(
			nameof(IFocusableElement.CancelledCommand), typeof(ICommand), typeof(IFocusableElement));

		public static readonly BindableProperty CancelledCommandParameterProperty = BindableProperty.Create(
			nameof(IFocusableElement.CancelledCommandParameter), typeof(object), typeof(IFocusableElement));

		public static readonly BindableProperty ReleasedOrCancelledCommandProperty = BindableProperty.Create(
			nameof(IFocusableElement.ReleasedOrCancelledCommand), typeof(ICommand), typeof(IFocusableElement));

		public static readonly BindableProperty ReleasedOrCancelledCommandParameterProperty = BindableProperty.Create(
			nameof(IFocusableElement.ReleasedOrCancelledCommandParameter), typeof(object), typeof(IFocusableElement));

        private static void OnIsFocusablePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IFocusableElement)bindable).OnIsFocusablePropertyChanged((bool)oldValue, (bool)newValue);
		}
	}
}
