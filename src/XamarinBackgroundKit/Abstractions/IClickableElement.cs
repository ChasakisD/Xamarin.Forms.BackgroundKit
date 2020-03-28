using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinBackgroundKit.Abstractions
{
	public interface IClickableElement
	{
		event EventHandler<EventArgs> Clicked;

		bool IsClickable { get; }

		ICommand ClickedCommand { get; }

		object ClickedCommandParameter { get; }

		void OnClicked();

		void OnIsClickablePropertyChanged(bool oldValue, bool newValue);
	}
}
